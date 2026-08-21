using MyHub.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyHub.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(ClaimsUserDTO<Guid> claims);
        string GenerateRefreshToken();

        ClaimsPrincipal? GetClaimsPrincipal(string? token);

        ClaimsUserDTO<TKey>? GetClaimsUserDTO<TKey>(ClaimsPrincipal claims);

        DateTimeOffset AccessTokenExpirationDate();
        DateTimeOffset RefreshTokenExpirationDate();
    }
}
