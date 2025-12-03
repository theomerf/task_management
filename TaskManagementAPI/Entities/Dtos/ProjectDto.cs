using Entities.Models;

namespace Entities.Dtos
{
    public class ProjectDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string Icon { get; init; } = null!;
        public string Color { get; init; } = null!;
        public ProjectStatus Status { get; init; }
        public int TaskCount { get; init; }
        public int CompletedTaskCount { get; init; }
        public DateTime CreatedAt { get; init; }
        public string CreatedByEmail { get; init; } = null!;
        public int MemberCount { get; init; }
    }
}
