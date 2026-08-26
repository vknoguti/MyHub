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
        public string DocumentType { get; set; } = default!;

        [Required]
        public string FileURL { get; set; } = default!;

        [Required]
        public string FileName { get; set; } = default!;

        public DateTimeOffset UploadedAt { get; set; } = default!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Profile Profile { get; set; } = default!;
    }
}
