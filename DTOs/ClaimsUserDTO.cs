using System;
using System.Security.Claims;

namespace MyHub.DTOs
{
    public class ClaimsUserDTO
    {
        public Guid IdUser { get; set; } = default!;
        public string UserName { get; set; } = default!;
    }
}