using MyHub.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyHub.Services.Token
{
    public interface ITokenService
    {
        string GenerateAccessToken(ClaimsUserDTO claims);
        string GenerateRefreshToken();

        ClaimsPrincipal? GetClaimsPrincipal(string? token);

        ClaimsUserDTO? GetClaimsUserDTO(ClaimsPrincipal claims);

        DateTimeOffset AccessTokenExpirationDate();
        DateTimeOffset RefreshTokenExpirationDate();
    }
}
