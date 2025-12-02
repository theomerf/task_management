using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Task
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TaskSequence { get; set; }
        public long? ProjectId { get; set; }
        public string? CreatedById { get; set; }
        public string? AssignedToId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.ToDo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public decimal? EstimatedHours { get; set; }
        public decimal TotalHoursSpent { get; set; } = 0;
        public int ProgressPercentage { get; set; } = 0;
        public DateTime? DeletedAt { get; set; }
        [NotMapped]
        public bool IsDeleted => DeletedAt.HasValue;
        public Project? Project { get; set; }
        public Account? CreatedBy { get; set; }
        public Account? AssignedTo { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Label> Labels { get; set; } = new List<Label>();
        public ICollection<TaskAttachment> Attachments { get; set; } = new List<TaskAttachment>();
        public ICollection<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();
        public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    }

    public enum TaskStatus
    {
        ToDo = 0,
        InProgress = 1,
        InReview = 2,
        Done = 3,
        Blocked = 4,
    }

    public enum TaskPriority
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Urgent = 3
    }
}