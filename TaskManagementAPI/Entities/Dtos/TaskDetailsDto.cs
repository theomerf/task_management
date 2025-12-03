using Entities.Models;

namespace Entities.Dtos
{
    public record TaskDetailsDto
    {
        public Guid Id { get; init; }
        public string CreatedByEmail { get; init; } = null!;
        public string CreatedByFirstName { get; init; } = null!;
        public string CreatedByLastName { get; init; } = null!;
        public string AssignedToEmail { get; init; } = null!;
        public string AssignedToFirstName { get; init; } = null!;
        public string AssignedToLastName { get; init; } = null!;
        public string LabelName { get; init; } = null!;
        public string LabelColor { get; init; } = null!;
        public string Title { get; init; } = null!;
        public string? Description { get; init; }
        public Entities.Models.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? DueDate { get; init; }
        public DateTime? CompletedAt { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
        public decimal? EstimatedHours { get; init; }
        public decimal TotalHoursSpent { get; init; } = 0;
        public int ProgressPercentage { get; init; }
        public int CommentCount { get; init; }
        public int AttachmentCount { get; init; }
    }
}
