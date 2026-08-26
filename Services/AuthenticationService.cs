using MyHub.Data;
using MyHub.DTOs;
using MyHub.DTOs.Mappings;
using MyHub.Entities;
using MyHub.Enums;
using MyHub.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyHub.DTOs.Authentication;
using MyHub.DTOs.AuthenticationService;


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

        public async Task<BaseResponse1<RegisterResponseDTO>> Register(RegisterUserDTO userRegister)
        {
            var registerResponse = new BaseResponse1<RegisterResponseDTO>();
            var queryUser =  _context.Users.AsQueryable().AsNoTracking();
            var userNameFound = await queryUser.AnyAsync(u => u.UserName == userRegister.UserName);
            if (userNameFound)
            {
                return registerResponse.GenerateResponse1<RegisterResponseDTO>(AppStatus.UsernameAlreadyExists);
            } 
            
            var emailFound = await queryUser.AnyAsync(u => u.Email == userRegister.Email);
            if (emailFound)
            {
                return registerResponse.GenerateResponse1<RegisterResponseDTO>(AppStatus.EmailAlreadyExists);
            }

            var userMapped = new User();
            userMapped = userRegister.ToUser(_passwordHasher.HashPassword(userMapped, userRegister.Password)) ?? 
                throw new ArgumentNullException(nameof(userRegister), "The mapping value was null");

            await _context.Users.AddAsync(userMapped);
            var success = await _context.SaveChangesAsync();
            
            if(success <= 0)
            {
                return registerResponse.GenerateResponse1<RegisterResponseDTO>(AppStatus.Failed);
            }

            return registerResponse.GenerateResponse1<RegisterResponseDTO>(AppStatus.SuccessRegistration);
        }

      
        public async Task<BaseResponse1<LoginResponseDTO>> Login(LoginUserDTO loginUser) 
        {
            var loginResponse = new BaseResponse1<LoginResponseDTO>();

            var targetUser = await _context.Users.FirstOrDefaultAsync(t => t.UserName == loginUser.UserName);
            if (targetUser is null)
            {
                return loginResponse.GenerateResponse1<LoginResponseDTO>(AppStatus.UserNotFound);
            }
            
            var matchPassword = _passwordHasher.VerifyHashedPassword(targetUser, targetUser.PasswordHash, loginUser.Password);
            if(matchPassword != PasswordVerificationResult.Success)
            {
                return loginResponse.GenerateResponse1<LoginResponseDTO>(AppStatus.InvalidCredentials);
            }

            var response = loginResponse.GenerateResponse1<LoginResponseDTO>(AppStatus.SuccessLogin);
            var accessToken = _tokenService.GenerateAccessToken(targetUser.ToClaimsUser());
            var refreshToken = _tokenService.GenerateRefreshToken();

            //TEORICAMENTE MEXENDO NO BANCO DO REDIS
            targetUser.RefreshToken = refreshToken;
            targetUser.RefreshTokenExpiryDate = _tokenService.RefreshTokenExpirationDate();
            await _context.SaveChangesAsync();

            response.Data = new LoginResponseDTO
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

        public async Task<BaseResponse1<RefreshTokenResponseDTO>> RenewJWTWithRefreshToken(RefreshTokenDTO refreshTokenDTO)
        {
            var response = new BaseResponse1<RefreshTokenResponseDTO>();
            if (refreshTokenDTO.RefreshToken is null)
            {
                return response.GenerateResponse1<RefreshTokenResponseDTO>(AppStatus.NullRefreshToken);
            }

            var targetUser = await _context.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshTokenDTO.RefreshToken);
            if(targetUser is null || targetUser.RefreshToken is null || targetUser.RefreshTokenExpiryDate < DateTimeOffset.UtcNow)
            {
                return response.GenerateResponse1<RefreshTokenResponseDTO>(AppStatus.InvalidRefreshToken);
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

            response = response.GenerateResponse1<RefreshTokenResponseDTO>(AppStatus.SuccessRenewAccessToken);
            response.Data = new RefreshTokenResponseDTO
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

        public async Task<BaseResponse1<LogOutResponseDTO>> LogOut(Guid userId)
        {
            var response = new BaseResponse1<LogOutResponseDTO>();
            var toLogOut = await _context.Users.FindAsync(userId);
            if(toLogOut is null)
            { 
                response.StatusCode = AppStatus.UserNotFound;
                return response;
            }

            response.Data = new LogOutResponseDTO { UserName = toLogOut.UserName };
            if (toLogOut.RefreshToken is null)
            {
                response.StatusCode = AppStatus.UserAlreadyLoggedOut;
                return response;
            }

            toLogOut.RefreshToken = null;
            toLogOut.RefreshTokenExpiryDate = null;
            var updated = await _context.SaveChangesAsync();

            if(updated <= 0)
            {
                response.StatusCode = AppStatus.FailedDatabaseUpdate;
                return response;
            }

            response.StatusCode = AppStatus.SuccessLogOut;
            return response;
        }
    }
}
