using MyHub.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyHub.DTOs.AuthenticationService
{
    public class DeleteUserResponseDTO
    {
        public Guid Id { get; set; } = default!;
        public string UserName { get; set; } = null!;  
        public string Email { get; set; } = null!;
    }
}
