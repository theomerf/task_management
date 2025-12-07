using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Attachment
    {
        public Guid Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AttachmentSequence { get; set; }
        public long TaskId { get; set; }
        public long? CommentId { get; set; }
        public string? UploadedById { get; set; }
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public long FileSize { get; set; }
        public string FileUrl { get; set; } = null!;
        public string? ThumbnailUrl { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        [NotMapped]
        public bool IsDeleted => DeletedAt.HasValue;
        public Task? Task { get; set; }
        public Comment? Comment { get; set; }
        public Account? UploadedBy { get; set; }
    }
}
