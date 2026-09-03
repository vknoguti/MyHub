namespace MyHub.DTOs.ProfileService
{
    public class CreateProfileDTO
    {
        public Guid UserId { get; set; } = default!;
        public string? FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
    }
}
