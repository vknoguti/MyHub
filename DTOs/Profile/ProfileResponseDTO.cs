namespace MyHub.DTOs.ProfileManagerService
{
    public class ProfileResponseDTO
    {
        public Guid Id { get; set; } = default!;
        public string? FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
    }
}
