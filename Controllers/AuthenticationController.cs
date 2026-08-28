using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHub.Data;
using MyHub.DTOs;
using MyHub.DTOs.Authentication;
using MyHub.DTOs.Common;
using MyHub.DTOs.Mappings;
using MyHub.Enums;
using MyHub.Services;
using MyHub.Services.Token;

namespace MyHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly ITokenService _tokenService;
        //REMOVER DEPOIS (Manutenção para testes)
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

            if (renewResponse.Error?.Type == ErrorType.NullRefreshToken ||
                renewResponse.Error?.Type == ErrorType.InvalidRefreshToken ||
                !renewResponse.IsSuccess)
            {
                return Unauthorized(ApiResponse.Fail("Sessão inválida ou expirada. Por favor, faça login novamente."));
            }

            var tokenDTO = renewResponse.Value?.TokenDTO;
            Response.Cookies.Append(nameof(TokenDTO.AccessToken), tokenDTO?.AccessToken ?? string.Empty,
                new CookieOptions
                {
                    Expires = tokenDTO?.AcessTokenExpiresAt,
                    HttpOnly = false,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

            Response.Cookies.Append(nameof(TokenDTO.RefreshToken), tokenDTO?.RefreshToken ?? string.Empty,
                new CookieOptions
                {
                    Expires = tokenDTO?.RefreshTokenExpiresAt,
                    HttpOnly = false,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

            return Ok(ApiResponse.Ok("Token renovado com sucesso."));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUser)
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return BadRequest(ApiResponse.Fail("Dados fornecidos são inválidos.", validationErrors));
            }

            var registerResponse = await _authService.Register(registerUser);

            if (registerResponse.Error?.Type == ErrorType.UsernameAlreadyExists ||
                registerResponse.Error?.Type == ErrorType.EmailAlreadyExists)
            {
                return Conflict(ApiResponse.Fail("Nome de usuário ou e-mail já cadastrado no sistema."));
            }

            if (!registerResponse.IsSuccess || registerResponse.Value is null)
            {
                return BadRequest(ApiResponse.Fail("Não foi possível concluir o cadastro. Verifique os dados e tente novamente."));
            }

            return StatusCode(StatusCodes.Status201Created,
                ApiResponse<RegisterResponseDTO>.Ok(registerResponse.Value, "Usuário cadastrado com sucesso."));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO login)
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return BadRequest(ApiResponse.Fail("Dados de login inválidos.", validationErrors));
            }

            var loginResponse = await _authService.Login(login);

            if (loginResponse.Error?.Type == ErrorType.UserNotFound ||
                loginResponse.Error?.Type == ErrorType.InvalidPassword ||
                loginResponse.Error?.Type == ErrorType.InvalidCredentials ||
                !loginResponse.IsSuccess)
            {
                return Unauthorized(ApiResponse.Fail("Usuário ou senha inválidos."));
            }

            var tokenDTO = loginResponse.Value?.TokenDTO;
            if (tokenDTO is not null)
            {
                Response.Cookies.Append(nameof(TokenDTO.AccessToken), tokenDTO.AccessToken,
                    new CookieOptions
                    {
                        Expires = tokenDTO.AcessTokenExpiresAt,
                        HttpOnly = false,
                        IsEssential = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });

                Response.Cookies.Append(nameof(TokenDTO.RefreshToken), tokenDTO.RefreshToken,
                    new CookieOptions
                    {
                        Expires = tokenDTO.RefreshTokenExpiresAt,
                        HttpOnly = false,
                        IsEssential = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });
            }

            return Ok(ApiResponse.Ok("Login realizado com sucesso."));
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            Request.Cookies.TryGetValue(nameof(TokenDTO.AccessToken), out var accessToken);
            var claims = _tokenService.GetClaimsPrincipal(accessToken) ?? User;
            if (claims is null)
            {
                return Unauthorized(ApiResponse.Fail("Usuário não autenticado."));
            }

            Response.Cookies.Delete(nameof(TokenDTO.AccessToken));
            Response.Cookies.Delete(nameof(TokenDTO.RefreshToken));

            var claimsUser = _tokenService.GetClaimsUserDTO(claims) ?? claims.ToClaimsUserDTO();
            if (claimsUser is null)
            {
                return Unauthorized(ApiResponse.Fail("Não foi possível identificar o usuário autenticado."));
            }

            var response = await _authService.LogOut(claimsUser.IdUser);

            if (!response.IsSuccess)
            {
                return BadRequest(ApiResponse.Fail("Não foi possível realizar o logout no momento."));
            }

            return Ok(ApiResponse.Ok("Logout realizado com sucesso."));
        }
    }
}
