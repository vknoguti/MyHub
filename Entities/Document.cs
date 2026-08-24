using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyHub.Entities
{
    public class Document<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; } = default!;

        public TKey UserId { get; set; } = default!;

        [Required]
        public string Title { get; set; } = default!;

        [Required]
        public byte[] Content { get; set; } = Array.Empty<byte>();


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public User<TKey> User { get; set; } = default!;
    }
}
