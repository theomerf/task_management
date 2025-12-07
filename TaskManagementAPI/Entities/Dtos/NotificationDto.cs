using Entities.Models;

namespace Entities.Dtos
{
    public record NotificationDto
    {
        public Guid Id { get; init; }
        public string RelatedTaskTitle { get; init; } = null!;
        public string RelatedProjectName { get; init; } = null!;
        public NotificationType Type { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? Icon { get; set; }
        public bool IsRead { get; set; } = false;
        public bool IsArchived { get; set; } = false;
        public DateTime CreatedAt { get; set; }
    }
}
