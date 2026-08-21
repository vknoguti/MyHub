namespace MyHub.DTOs
{
    public class TokenDTO
    {
        public string AccessToken { get; set; } = default!;
        public DateTimeOffset? AcessTokenExpiresAt { get; set; } = default!;
        
        public string RefreshToken { get; set; } = default!;
        public DateTimeOffset? RefreshTokenExpiresAt { get; set; } = default!;
    }
}
