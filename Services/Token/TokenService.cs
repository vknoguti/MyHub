using MyHub.DTOs;
using MyHub.Enums;
using MyHub.Extensions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MyHub.Services.Token;

namespace MyHub.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly string _secretKey;
        private readonly double _accessTokenExpiryMinutes;
        private readonly double _refreshTokenExpiryDays;

        private readonly double _refreshTokenExpiryMinutes;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _secretKey = config["JWT:SecretKey"] ?? throw new InvalidOperationException("Invalid Secret Key");
            _accessTokenExpiryMinutes = _config.GetSection("JWT").GetValue<double>("AccessTokenExpiryMinutes", 60.0);
            _refreshTokenExpiryDays = _config.GetSection("JWT").GetValue<double>("RefreshTokenValidityDays", 7.0);

            _refreshTokenExpiryMinutes = _config.GetSection("JWT").GetValue<double>("RefreshTokenValidityMinutes", 1.0);
        }

        public DateTimeOffset AccessTokenExpirationDate()
        {
            return DateTimeOffset.UtcNow.AddMinutes(_accessTokenExpiryMinutes);
        }

        public string GenerateAccessToken(ClaimsUserDTO claimsUser)
        {
            var issuer = _config["JWT:Issuer"] ?? throw new InvalidOperationException("Invalid Issuer");
            var audience = _config["JWT:Audience"] ?? throw new InvalidOperationException("Invalid Audience");

            var claims = new[]
            { 
                new Claim(MyClaimTypes.IdUser, claimsUser.IdUser.ToString()),
                new Claim(MyClaimTypes.UserName, claimsUser.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_accessTokenExpiryMinutes),
                Audience = audience,
                Issuer = issuer,
                SigningCredentials = credential
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        public string GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[128];

            using var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(secureRandomBytes);

            var refreshToken = Convert.ToBase64String(secureRandomBytes);
            return refreshToken;
        }

        public ClaimsPrincipal? GetClaimsPrincipal(string? token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return default;
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = false,

                //MUDAR AQUI
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
            };

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            
            if(securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }
            return principal;
        }

        public ClaimsUserDTO? GetClaimsUserDTO(ClaimsPrincipal claims) 
        {
            var couldParse = Guid.TryParse(claims.FindFirstValue(nameof(ClaimsUserDTO.IdUser)), out var idUser);
            var userName = claims.FindFirstValue(nameof(ClaimsUserDTO.UserName));
            if (!couldParse || userName is null) return null;

            return new ClaimsUserDTO
            {
                IdUser = idUser,
                UserName = userName
            };
        }
   
        public DateTimeOffset RefreshTokenExpirationDate()
        {
            //return DateTimeOffset.UtcNow.AddDays((double)_refreshTokenExpiryDays);
            return DateTimeOffset.UtcNow.AddMinutes(_refreshTokenExpiryMinutes);
        }
    }
}
