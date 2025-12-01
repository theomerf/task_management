using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Mention
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MentionSequence { get; set; }
        public long CommentId { get; set; }
        public string? MentionedUserId { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime MentionedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }
        public Comment? Comment { get; set; }
        public Account? MentionedUser { get; set; }
    }
}
