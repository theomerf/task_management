using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Comment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CommentSequence { get; set; }
        public long TaskId { get; set; }
        public string? AuthorId { get; set; }
        public long? ParentCommentId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        [NotMapped]
        public bool IsDeleted => DeletedAt.HasValue;
        public string? Reactions { get; set; }
        public Task? Task { get; set; }
        public Account? Author { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public ICollection<Mention> Mentions { get; set; } = new List<Mention>();
    }
}
