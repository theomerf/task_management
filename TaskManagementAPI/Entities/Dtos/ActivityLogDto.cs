using Entities.Models;

namespace Entities.Dtos
{
    public record ActivityLogDto
    {
        public Guid Id { get; init; }
        public string PerformedByEmail { get; init; } = null!;
        public string PerformedByFirstName { get; init; } = null!;
        public string PerformedByLastName { get; init; } = null!;
        public ActivityType Type { get; init; }
        public string Description { get; init; } = null!;
        public string? OldValue { get; init; }
        public string? NewValue { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }
}
