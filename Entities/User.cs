using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace MyHub.Entities
{
    public class User<TKey> where TKey : IEquatable<TKey>, IEntity<TKey>
    {
        public TKey Id { get; set; } = default!;

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string UserName { get; set; } = null!;

        [Column(TypeName = "nvarchar(60)")]
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string PasswordHash { get; set; } = null!;

        [Column(TypeName = "nvarchar(max)")]
        public string? RefreshToken { get; set; }

        [Column(TypeName = "datetimeoffset(7)")]
        public DateTimeOffset? RefreshTokenExpiryDate { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Profile<TKey> Profile { get; set; } = default!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<Document<TKey>>? Documents { get; set;  }
    }
}
