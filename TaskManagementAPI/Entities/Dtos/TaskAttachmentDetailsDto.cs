using Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Dtos
{
    public record TaskAttachmentDetailsDto
    {
        public Guid Id { get; set; }
        public string UploadedByEmail { get; init; } = null!;
        public string UploadedByFirstName { get; set; } = null!;
        public string UploadedByLastName { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public long FileSize { get; set; }
        public string FileUrl { get; set; } = null!;
        public string? ThumbnailUrl { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
