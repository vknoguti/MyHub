using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace MyHub.Entities
{
    public class Profile<TKey> where TKey : IEquatable<TKey>, IEntity<TKey>
    {
        public TKey Id { get; set; } = default!;
        public TKey UserId { get; set; } = default!;

        [Column(TypeName = "nvarchar(200)")]
        public string? FullName { get; set; } = null!;

        [Column(TypeName = "nvarchar(50)")]
        public string? PhoneNumber { get; set; }

        public DateTime? BirthDate { get; set; }


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public User<TKey> User { get; set; } = default!;
    }
}
