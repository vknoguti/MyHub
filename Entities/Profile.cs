using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace MyHub.Entities
{
    public class Profile
    {
        public Guid Id { get; set; } = default!;
        public Guid UserId { get; set; } = default!;

        [Column(TypeName = "nvarchar(200)")]
        public string? FullName { get; set; } = null!;

        [Column(TypeName = "nvarchar(50)")]
        public string? PhoneNumber { get; set; }

        public DateTimeOffset? BirthDate { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<Document>? Documents { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public User User { get; set; } = default!;
    }
}
