using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHub.Data;
using MyHub.DTOs;
using MyHub.DTOs.Common;
using MyHub.DTOs.Mappings;
using MyHub.DTOs.ProfileManagerService;
using MyHub.DTOs.ProfileService;
using MyHub.Enums;
using MyHub.Services;
using MyHub.Services.Token;
using System.Security.Claims;

namespace MyHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileManagerService _profileManagerService;
        private readonly ITokenService _tokenService;
        //REMOVER DEPOIS (Manutenção para testes)
        private readonly ApplicationDbContext _context;

        public ProfileController(ProfileManagerService profileManagerService, ApplicationDbContext context, ITokenService tokenService)
        {
            _profileManagerService = profileManagerService;
            _context = context;
            _tokenService = tokenService;
        }

        [Authorize]
        [HttpPost("register-profile")]
        public async Task<IActionResult> CreateProfile([FromBody] CreateProfileDTO profileDTO)
        {
            Request.Cookies.TryGetValue(nameof(TokenDTO.AccessToken), out var accessToken);
            var claims = _tokenService.GetClaimsPrincipal(accessToken) ?? User;
            if (claims is null)
            {
                return Unauthorized(ApiResponse.Fail("Usuário não autenticado."));
            }

            var claimsUser = _tokenService.GetClaimsUserDTO(claims) ?? claims.ToClaimsUserDTO();
            if (claimsUser is null)
            {
                return Unauthorized(ApiResponse.Fail("Não foi possível identificar o usuário autenticado."));
            }

            profileDTO.UserId = claimsUser.IdUser;
            var responseRegister = await _profileManagerService.CreateProfile(profileDTO);

            if (responseRegister.Error?.Type == ErrorType.ProfileAlreadyExists)
            {
                return Conflict(ApiResponse.Fail("O usuário já possui um perfil cadastrado."));
            }

            if (responseRegister.Error?.Type == ErrorType.UserNotFound)
            {
                return NotFound(ApiResponse.Fail("Usuário não encontrado."));
            }

            if (!responseRegister.IsSuccess || responseRegister.Value is null)
            {
                return BadRequest(ApiResponse.Fail("Não foi possível cadastrar o perfil. Verifique os dados informados."));
            }

            return StatusCode(StatusCodes.Status201Created,
                ApiResponse<ProfileResponseDTO>.Ok(responseRegister.Value, "Perfil criado com sucesso."));
        }

        [Authorize]
        [HttpPost("get-profile")]
        public async Task<IActionResult> GetProfile([FromBody] ProfileRefDTO profile)
        {
            Request.Cookies.TryGetValue(nameof(TokenDTO.AccessToken), out var accessToken);
            var claims = _tokenService.GetClaimsPrincipal(accessToken) ?? User;
            if (claims is null)
            {
                return Unauthorized(ApiResponse.Fail("Usuário não autenticado."));
            }

            var claimsUser = _tokenService.GetClaimsUserDTO(claims) ?? claims.ToClaimsUserDTO();
            if (claimsUser is null)
            {
                return Unauthorized(ApiResponse.Fail("Não foi possível identificar o usuário autenticado."));
            }

            if (claimsUser.IdUser != profile.UserId)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    ApiResponse.Fail("Acesso não autorizado ao perfil solicitado."));
            }

            var response = await _profileManagerService.GetProfile(profile);
            if (response.Error?.Type == ErrorType.ProfileNotFound || !response.IsSuccess || response.Value is null)
            {
                return NotFound(ApiResponse.Fail("Perfil não encontrado."));
            }

            return Ok(ApiResponse<ProfileResponseDetailedDTO>.Ok(response.Value, "Perfil recuperado com sucesso."));
        }

        [HttpGet("list-profiles")]
        public IActionResult ListProfiles()
        {
            return Ok(_context.Profiles);
        }

        [HttpGet("list-users")]
        public IActionResult ListUsers()
        {
            return Ok(_context.Users);
        }

        [HttpGet("list-token-data")]
        public IActionResult ListTokenData()
        {
            Request.Cookies.TryGetValue(nameof(TokenDTO.AccessToken), out var accessToken);
            var claims = _tokenService.GetClaimsPrincipal(accessToken);
            var idUser = claims?.FindFirstValue("IdUser");
            var userName = claims?.FindFirstValue("UserName");

            return Ok(new { idUser, userName });
        }
    }
}
