using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHub.Data;
using MyHub.DTOs;
using MyHub.DTOs.Authentication;
using MyHub.DTOs.Common;
using MyHub.DTOs.Mappings;
using MyHub.Entities;
using MyHub.Enums;
using MyHub.Services;
using MyHub.Services.Authentication;
using MyHub.Services.Token;
using MyHub.Shared;

namespace MyHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(IAuthenticationService authService, ApplicationDbContext context, ITokenService tokenService) : ControllerBase
    {
        private readonly IAuthenticationService _authService = authService;
        private readonly ITokenService _tokenService = tokenService;
        //REMOVER DEPOIS (Manutenção para testes)
        private readonly ApplicationDbContext _context = context;

        [Authorize]
        [HttpGet("verify-authorization")]
        public IActionResult VerifyAuthorization()
        {
            Request.Cookies.TryGetValue(nameof(TokenDTO.AccessToken), out var accessToken);
            Request.Cookies.TryGetValue(nameof(TokenDTO.RefreshToken), out var refreshToken);
            return Ok($"You have the token JWT with active access\n" +
                      $"Access Token: {accessToken}\n" +
                      $"Refresh Token: {refreshToken}");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RenewTokenJWT()
        {
            Request.Cookies.TryGetValue(nameof(TokenDTO.RefreshToken), out var refreshToken);

            var renewResponse = await _authService.RenewJWTWithRefreshToken(
                new RefreshTokenDTO { RefreshToken = refreshToken });

            if (renewResponse.Error?.Type == ErrorType.NullRefreshToken ||
                renewResponse.Error?.Type == ErrorType.InvalidRefreshToken ||
                !renewResponse.IsSuccess)
            {
                return Unauthorized(ApiResponse.Fail("Invalid or expired session, please log in again."));
            }

            var tokenDTO = renewResponse.Value?.TokenDTO;
            Response.Cookies.Append(nameof(TokenDTO.AccessToken), tokenDTO?.AccessToken ?? string.Empty,
                new CookieOptions
                {
                    Expires = tokenDTO?.AcessTokenExpiresAt,
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

            Response.Cookies.Append(nameof(TokenDTO.RefreshToken), tokenDTO?.RefreshToken ?? string.Empty,
                new CookieOptions
                {
                    Expires = tokenDTO?.RefreshTokenExpiresAt,
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

            return Ok(ApiResponse.Ok("Token renewed sucessfully."));
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserDTO registerUser)
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return BadRequest(ApiResponse.Fail("Invalid data provided.", validationErrors));
            }

            var registerResponse = await _authService.Register(registerUser);

            if (registerResponse.Error?.Type == ErrorType.UsernameAlreadyExists ||
                registerResponse.Error?.Type == ErrorType.EmailAlreadyExists)
            {
                return Conflict(ApiResponse.Fail("Username or email is already registered."));
            }

            if (!registerResponse.IsSuccess || registerResponse.Value is null)
            {
                return BadRequest(ApiResponse.Fail("Could not complete the registration. Please check your data and try again."));
            }

            return StatusCode(StatusCodes.Status201Created,
                    ApiResponse<RegisterUserResponseDTO>.Ok(registerResponse.Value!, "User registered successfully."));

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO login)
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return BadRequest(ApiResponse.Fail("Invalid login credentials.", validationErrors));
            }

            var loginResponse = await _authService.Login(login);

            if (loginResponse.Error?.Type == ErrorType.UserNotFound ||
                loginResponse.Error?.Type == ErrorType.InvalidPassword ||
                loginResponse.Error?.Type == ErrorType.InvalidCredentials ||
                !loginResponse.IsSuccess)
            {
                return Unauthorized(ApiResponse.Fail("Invalid username or password."));
            }

            var tokenDTO = loginResponse.Value?.TokenDTO;
            if (tokenDTO is not null)
            {
                Response.Cookies.Append(nameof(TokenDTO.AccessToken), tokenDTO.AccessToken,
                    new CookieOptions
                    {
                        Expires = tokenDTO.AcessTokenExpiresAt,
                        HttpOnly = true,
                        IsEssential = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });

                Response.Cookies.Append(nameof(TokenDTO.RefreshToken), tokenDTO.RefreshToken,
                    new CookieOptions
                    {
                        Expires = tokenDTO.RefreshTokenExpiresAt,
                        HttpOnly = true,
                        IsEssential = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });
            }

            return Ok(ApiResponse.Ok("Login successful."));
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            //Request.Cookies.TryGetValue(nameof(TokenDTO.AccessToken), out var accessToken);
            var claims = User; //?? _tokenService.GetClaimsPrincipal(accessToken);
            if (claims is null)
            {
                return Unauthorized(ApiResponse.Fail("User is not authenticated."));
            }

            Response.Cookies.Delete(nameof(TokenDTO.AccessToken));
            Response.Cookies.Delete(nameof(TokenDTO.RefreshToken));

            var claimsUser = _tokenService.GetClaimsUserDTO(claims) ?? claims.ToClaimsUserDTO();
            if (claimsUser is null)
            {
                return Unauthorized(ApiResponse.Fail("Could not identify the authenticated user."));
            }

            var response = await _authService.LogOut(claimsUser.IdUser);

            if (!response.IsSuccess)
            {
                return BadRequest(ApiResponse.Fail("Could not log out at this time."));
            }

            return Ok(ApiResponse.Ok("Logout successful."));
        }

        [Authorize]
        [HttpDelete("delete-user")]
        public async Task<IActionResult> DeleteUser([FromQuery] Guid userId)
        {
            var claims = User;
            if (claims is null)
            {
                return Unauthorized(ApiResponse.Fail("User not authenticated."));
            }

            var response = await _authService.DeleteUser(userId);
            if(response.Error?.Type == ErrorType.UserNotFound)
            {
                return NotFound(ApiResponse.Fail("User not found."));
            }

            if(response.Error?.Type == ErrorType.FailedDatabaseUpdate)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.Fail("An unexpected error occurred"));
            }

            return Ok(ApiResponse.Ok("User deleted succesfully."));
        }
    }
}
