using System;
using System.Security.Claims;

namespace MyHub.DTOs
{
    public class ClaimsUserDTO<TKey> 
    {
        public TKey IdUser { get; set; } = default!;
        public string UserName { get; set; } = default!;
    }
}