using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NotificationSequence { get; set; }
        public string RecipientId { get; set; } = null!;
        public string? InitiatorId { get; set; }
        public long? RelatedTaskId { get; set; }
        public long? RelatedProjectId { get; set; }
        public NotificationType Type { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? Icon { get; set; }
        public bool IsRead { get; set; } = false;
        public bool IsArchived { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }
        public Account? Recipient { get; set; }
        public Account? Initiator { get; set; }
        public Task? RelatedTask { get; set; }
        public Project? RelatedProject { get; set; }
    }

    public enum NotificationType
    {
        TaskAssigned = 0,
        TaskCompleted = 1,
        CommentAdded = 2,
        MentionAdded = 3,
        TaskDueSoon = 4,
        TaskOverdue = 5,
        ProjectInvitation = 6,
        ProjectUpdated = 7,
        TeamMemberAdded = 8,
    }
}
