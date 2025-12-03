using Entities.Models;

namespace Entities.Dtos
{
    public record TaskDto
    {
        public Guid Id { get; init; }
        public string CreatedByEmail { get; init; } = null!;
        public string AssignedToEmail { get; init; } = null!;
        public string Title { get; init; } = null!;
        public Entities.Models.TaskStatus Status { get; init; }
        public TaskPriority Priority { get; init; }
        public int ProgressPercentage { get; init; }
        public int CommentCount { get; init; }
        public int AttachmentCount { get; init; }
    }
}
