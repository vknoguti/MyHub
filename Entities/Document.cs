using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyHub.Entities
{
    public class Document<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; } = default!;

        [Required]
        public TKey ProfileId { get; set; } = default!;

        [Required]
        public string DocumentType { get; set; } = default!;

        [Required]
        public string FileURL { get; set; } = default!;

        [Required]
        public string FileName { get; set; } = default!;

        public DateTimeOffset UploadedAt { get; set; } = default!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Profile<TKey> Profile { get; set; } = default!;
    }
}
