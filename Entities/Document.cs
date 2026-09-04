using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyHub.Entities
{
    public class Document
    {
        public Guid Id { get; set; } = default!;

        [Required]
        public Guid ProfileId { get; set; } = default!;

        [Required]
        public string ContentType { get; set; } = default!;

        [Required]
        public string StorageKey { get; set; } = default!;

        [Required]
        public string FileName { get; set; } = default!;

        [Required]
        public long FileSizeBytes { get; set; } 

        [Required]
        public DateTimeOffset UploadedAt { get; set; } = default!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Profile Profile { get; set; } = default!;
    }
}
