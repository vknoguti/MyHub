using Microsoft.AspNetCore.Mvc;
using MyHub.Data;
using MyHub.DTOs;
using MyHub.Repository;
using MyHub.Services;
using System.Security.Claims;

namespace MyHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        public ProfileManagerService<Guid> _profileManagerService;
        public ITokenService _tokenService;

        public ApplicationDbContext _context;
        public ProfileController(ProfileManagerService<Guid> profileManagerService, ApplicationDbContext context, ITokenService tokenService)
        {
            _profileManagerService = profileManagerService;
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register-profile")]
        public async Task<IActionResult> CreateProfile([FromBody] RegisterProfileDTO<Guid> profileDTO)
        {
            Request.Cookies.TryGetValue(nameof(TokenDTO.AccessToken), out var accessToken);
            var claims = _tokenService.GetClaimsPrincipal(accessToken);
            if(claims is null)
            {
                //ADICIONAR O STATUS CODE CERTO DEPOIS
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            var claimsUser = _tokenService.GetClaimsUserDTO<Guid>(claims);
            if(claimsUser is null)
            {
                return BadRequest();
            }
            
            var userId = claimsUser.IdUser;

            var profileDTOCreated = await _profileManagerService.Register(profileDTO);
            return Ok(profileDTOCreated);
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
            
            return Ok(new { idUser, userName});
        }
    }
}
