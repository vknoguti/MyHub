using MyHub.Data;
using MyHub.DTOs;
using MyHub.DTOs.Mappings;
using MyHub.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyHub.DTOs.Authentication;
using MyHub.DTOs.AuthenticationService;
using MyHub.Services.Token;
using MyHub.Shared;
using MyHub.Services.Authentication;


namespace MyHub.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;
        public AuthenticationService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, ITokenService tokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<Result<RegisterUserResponseDTO>> Register(RegisterUserDTO userRegister)
        {
            var registerResponse = new BaseResponse1<RegisterUserResponseDTO>();

            var queryUser = _context.Users.AsQueryable().AsNoTracking();
            var userNameExists = await queryUser.AnyAsync(u => u.UserName == userRegister.UserName);
            if (userNameExists)
            {
                return AuthenticationErrors.UsernameAlreadyExists;
            }

            var emailExists = await queryUser.AnyAsync(u => u.Email == userRegister.Email);
            if (emailExists)
            {
                return AuthenticationErrors.EmailAlreadyExists;
            }

            var userMapped = new User();
            userMapped = userRegister.ToUser(_passwordHasher.HashPassword(userMapped, userRegister.Password)) ??
                throw new ArgumentNullException(nameof(userRegister), "The mapping value was null");

            await _context.Users.AddAsync(userMapped);
            var success = await _context.SaveChangesAsync();

            if (success <= 0)
            {
                return AuthenticationErrors.UserRegistrationFailed;
            }

            return userMapped.ToRegisterResponseDTO();
        }


        public async Task<Result<LoginUserResponseDTO>> Login(LoginUserDTO loginUser)
        {
            var loginResponse = new BaseResponse1<LoginUserResponseDTO>();

            var targetUser = await _context.Users.FirstOrDefaultAsync(t => t.UserName == loginUser.UserName);
            if (targetUser is null)
            {
                return AuthenticationErrors.UserNotFound;
            }

            var matchPassword = _passwordHasher.VerifyHashedPassword(targetUser, targetUser.PasswordHash, loginUser.Password);
            if (matchPassword != PasswordVerificationResult.Success)
            {
                return AuthenticationErrors.InvalidPassword;
            }

      
            var accessToken = _tokenService.GenerateAccessToken(targetUser.ToClaimsUser());
            var refreshToken = _tokenService.GenerateRefreshToken();

            //TEORICAMENTE MEXENDO NO BANCO DO REDIS
            targetUser.RefreshToken = refreshToken;
            targetUser.RefreshTokenExpiryDate = _tokenService.RefreshTokenExpirationDate();
            await _context.SaveChangesAsync();

            var response = new LoginUserResponseDTO
            {
                TokenDTO = new TokenDTO
                {
                    AccessToken = accessToken,
                    AcessTokenExpiresAt = _tokenService.AccessTokenExpirationDate(),
                    RefreshToken = refreshToken,
                    RefreshTokenExpiresAt = _tokenService.RefreshTokenExpirationDate()
                }
            };
            return response;
        }

        public async Task<Result<RefreshTokenResponseDTO>> RenewJWTWithRefreshToken(RefreshTokenDTO refreshTokenDTO)
        {
            if (refreshTokenDTO.RefreshToken is null)
            {
                return AuthenticationErrors.NullRefreshToken;
            }

            var targetUser = await _context.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshTokenDTO.RefreshToken);
            if (targetUser is null || targetUser.RefreshToken is null || targetUser.RefreshTokenExpiryDate < DateTimeOffset.UtcNow)
            {
                return AuthenticationErrors.InvalidRefreshToken;
            }

            var claimsUser = targetUser.ToClaimsUser();
            var accessToken = _tokenService.GenerateAccessToken(claimsUser);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var principals = _tokenService.GetClaimsPrincipal(accessToken);

            var refreshTokenExpirationDate = _tokenService.RefreshTokenExpirationDate();
            var accessTokenExpirationDate = _tokenService.AccessTokenExpirationDate();

            var expirationClaimIsConverted = long.TryParse(principals?.FindFirst("exp")?.Value, out var expirationClaim);
            accessTokenExpirationDate = expirationClaimIsConverted ? DateTimeOffset.FromUnixTimeSeconds(expirationClaim) : accessTokenExpirationDate;

            targetUser.RefreshToken = refreshToken;
            targetUser.RefreshTokenExpiryDate = refreshTokenExpirationDate;
            await _context.SaveChangesAsync();

            var response = new RefreshTokenResponseDTO
            {
                TokenDTO = new TokenDTO
                {
                    AccessToken = accessToken,
                    AcessTokenExpiresAt = accessTokenExpirationDate,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiresAt = refreshTokenExpirationDate
                }
            };
            return response;
        }

        public async Task<Result<LogOutResponseDTO>> LogOut(Guid userId)
        {
            var response = new BaseResponse1<LogOutResponseDTO>();
            var toLogOut = await _context.Users.FindAsync(userId);
            if (toLogOut is null)
            {
                return AuthenticationErrors.UserNotFound;
            }

            response.Data = new LogOutResponseDTO { UserName = toLogOut.UserName };
            if (toLogOut.RefreshToken is null)
            {
                return AuthenticationErrors.UserAlreadyLoggedOut;
            }

            toLogOut.RefreshToken = null;
            toLogOut.RefreshTokenExpiryDate = null;
            var updated = await _context.SaveChangesAsync();

            if (updated <= 0)
            {
                return AuthenticationErrors.FailedDatabaseUpdate;
            }

            return new LogOutResponseDTO { UserName = toLogOut.UserName };
        }

        public async Task<Result<DeleteUserResponseDTO>> DeleteUser(Guid userId)
        {
            var toDelete = await _context.Users.SingleOrDefaultAsync(t => t.Id == userId);
            if(toDelete is null)
            {
                return AuthenticationErrors.UserNotFound;
            }

            _context.Users.Remove(toDelete);
            var isRemoved = await _context.SaveChangesAsync();
            if(isRemoved <= 0)
            {
                return AuthenticationErrors.FailedDatabaseUpdate;
            }

            return toDelete.ToDeleteUserResponseDTO();
        }
    }
}
