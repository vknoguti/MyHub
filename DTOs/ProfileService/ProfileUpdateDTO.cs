namespace MyHub.DTOs.ProfileService
{
    public class ProfileUpdateDTO
    {
        public Guid Id { get; set; } = default!;
        public Guid UserId { get; set; } = default!;
        public string? FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
    }
}
