using MyHub.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyHub.DTOs
{
    public class RegisterResponseDTO
    {
        public Guid Id { get; set; } = default!;

        public string UserName { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;


        public string? PhoneNumber { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
