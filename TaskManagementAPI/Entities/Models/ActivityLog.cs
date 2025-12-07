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
        TaskPriorityChanged = 3,
        TaskAssigned = 4,
        TaskCompleted = 5,
        TaskDeleted = 9,

        ProjectCreated = 10,
        ProjectUpdated = 11,
        ProjectArchived = 12,
        ProjectRestored = 13,
        ProjectCompleted = 14,
        ProjectDeleted = 19,

        MemberAdded = 20,
        MemberRoleChanged = 21,
        MemberRemoved = 29,

        CommentAdded = 30,
        CommentUpdated = 31,
        CommentDeleted = 39,

        TimeLogAdded = 40
    }
}
