using MyHub.Enums;
using System.Security.Claims;

namespace MyHub.DTOs.Mappings
{
    public static class ClaimsPrincipalExtension
    {
        public static ClaimsUserDTO<TKey>? ToClaimsUserDTO<TKey>(this ClaimsPrincipal claimsPrincipal) where TKey: IEquatable<TKey>, IParsable<TKey>
        {
            string? idUser = claimsPrincipal.FindFirstValue(MyClaimTypes.IdUser);
            string? userName = claimsPrincipal.FindFirstValue(MyClaimTypes.UserName);
            if (idUser is null || userName is null) return null;

            return new ClaimsUserDTO<TKey>
            {
                IdUser = TKey.Parse(idUser, null),
                UserName = userName
            };
        }
    }
}
