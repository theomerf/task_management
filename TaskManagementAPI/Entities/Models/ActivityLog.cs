using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class ActivityLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ActivityLogSequence { get; set; }   
        public string? PerformedById { get; set; }
        public long? RelatedTaskId { get; set; }
        public long? RelatedProjectId { get; set; }
        public ActivityType Type { get; set; }
        public string Description { get; set; } = null!;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Account? PerformedBy { get; set; }
        public Task? RelatedTask { get; set; }
        public Project? RelatedProject { get; set; }
    }

    public enum ActivityType
    {
        TaskCreated = 0,
        TaskUpdated = 1,
        TaskStatusChanged = 2,
        TaskAssigned = 3,
        TaskCompleted = 4,
        TaskDeleted = 5,

        ProjectCreated = 10,
        ProjectUpdated = 11,
        ProjectArchived = 12,
        ProjectDeleted = 13,

        MemberAdded = 20,
        MemberRemoved = 21,
        MemberRoleChanged = 22,

        CommentAdded = 30,
        CommentUpdated = 31,
        CommentDeleted = 32,

        TimeLogAdded = 40
    }
}
