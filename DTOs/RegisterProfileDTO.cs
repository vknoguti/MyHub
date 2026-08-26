using MyHub.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyHub.DTOs
{
    public class RegisterProfileDTO
    {
        public Guid UserId { get; set; } = default!; 
        public string? FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
    }
}
