namespace MyHub.DTOs.ProfileManagerService
{
    public class ProfileResponseDTO
    {
        public string? FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
    }
}
