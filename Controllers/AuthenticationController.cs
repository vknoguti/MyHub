using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHub.Data;
using MyHub.DTOs;
using MyHub.DTOs.Authentication;
using MyHub.Enums;
using MyHub.Extensions;
using MyHub.Services;

namespace MyHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly ITokenService _tokenService;
        //REMOVER DEPOIS
        private readonly ApplicationDbContext _context;
        public AuthenticationController(IAuthenticationService authService, ApplicationDbContext context, ITokenService tokenService)
        {
            _authService = authService;
            _context = context;
            _tokenService = tokenService;
        }
       
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

            if(renewResponse.StatusCode == AppStatus.NullRefreshToken
                || renewResponse.StatusCode == AppStatus.InvalidRefreshToken)
            {
                return BadRequest(renewResponse);
            }

            var tokenDTO = renewResponse?.Data?.TokenDTO;
            this.Response.Cookies.Append(nameof(TokenDTO.AccessToken), tokenDTO?.AccessToken ?? string.Empty,
                new CookieOptions
                {
                    Expires = tokenDTO?.AcessTokenExpiresAt,
                    //MUDAR AQUI
                    HttpOnly = false,
                    IsEssential = true,
                    Secure = true,
                    //MUDAR AQUI
                    SameSite = SameSiteMode.Strict
                });
            this.Response.Cookies.Append(nameof(TokenDTO.RefreshToken), tokenDTO?.RefreshToken ?? string.Empty,
                new CookieOptions
                {
                    Expires = tokenDTO?.RefreshTokenExpiresAt,
                    //MUDAR AQUI
                    HttpOnly = false,
                    IsEssential = true,
                    Secure = true,
                    //MUDAR AQUI
                    SameSite = SameSiteMode.Strict
                });
            return Ok(renewResponse);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));
            }

            var registerResponse = await _authService.Register(registerUser);
            if(registerResponse.StatusCode == AppStatus.UsernameAlreadyExists)
            {
                return BadRequest(registerResponse);
            }
            if(registerResponse.StatusCode == AppStatus.EmailAlreadyExists)
            {
                return BadRequest(registerResponse);
            }
            return Ok(registerResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO login)
        {
            var loginResponse = await _authService.Login(login);
        
            if(loginResponse.StatusCode == AppStatus.UserNotFound)
            {
                return NotFound(loginResponse);
            }

            if(loginResponse.StatusCode == AppStatus.InvalidCredentials)
            {
                return BadRequest(loginResponse);
            }

            var tokenDTO = loginResponse?.Data?.TokenDTO;

            this.Response.Cookies.Append(nameof(TokenDTO.AccessToken), tokenDTO!.AccessToken,
                new CookieOptions
                {
                    Expires = tokenDTO?.AcessTokenExpiresAt,
                    //MUDAR AQUI
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    //MUDAR AQUI
                    SameSite = SameSiteMode.Strict
                });

            this.Response.Cookies.Append(nameof(TokenDTO.RefreshToken), tokenDTO!.RefreshToken,
                new CookieOptions
                {
                    Expires = tokenDTO?.RefreshTokenExpiresAt,
                    //MUDAR AQUI
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    //MUDAR AQUI
                    SameSite = SameSiteMode.Strict
                });
            return Ok(loginResponse);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            Request.Cookies.TryGetValue(nameof(TokenDTO.AccessToken), out var accessToken);
            var claims = _tokenService.GetClaimsPrincipal(accessToken);
            if (claims is null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new BaseResponse1<object>().GenerateResponse1<object>(status: AppStatus.CredentialsNotFound));
            }

            Response.Cookies.Delete(nameof(TokenDTO.AccessToken));
            Response.Cookies.Delete(nameof(TokenDTO.RefreshToken));

            var claimsUser = _tokenService.GetClaimsUserDTO(claims);
            if (claimsUser is null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new BaseResponse1<object>().GenerateResponse1<object>(status: AppStatus.PrincipalsNotFound));
            }

            var response = await _authService.LogOut(claimsUser.IdUser);
            if(response.StatusCode == AppStatus.UserNotFound) { 
            }

            if(response.StatusCode == AppStatus.UserNotFound ||
                response.StatusCode == AppStatus.UserAlreadyLoggedOut ||
                response.StatusCode == AppStatus.FailedDatabaseUpdate)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
