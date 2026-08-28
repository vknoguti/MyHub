using MyHub.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyHub.DTOs.ProfileService
{
    public class ProfileResponseDetailedDTO
    {
        public Guid Id { get; set; } = default!;
        public Guid UserId { get; set; } = default!;
        public string? FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
    }
}
