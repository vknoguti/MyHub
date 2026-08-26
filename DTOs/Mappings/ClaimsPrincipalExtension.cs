using MyHub.Enums;
using System.Security.Claims;

namespace MyHub.DTOs.Mappings
{
    public static class ClaimsPrincipalExtension
    {
        public static ClaimsUserDTO? ToClaimsUserDTO(this ClaimsPrincipal claimsPrincipal) 
        {
            string? idUser = claimsPrincipal.FindFirstValue(MyClaimTypes.IdUser);
            string? userName = claimsPrincipal.FindFirstValue(MyClaimTypes.UserName);
            if (idUser is null || userName is null) return null;

            return new ClaimsUserDTO
            {
                IdUser = Guid.Parse(idUser, null),
                UserName = userName
            };
        }
    }
}
