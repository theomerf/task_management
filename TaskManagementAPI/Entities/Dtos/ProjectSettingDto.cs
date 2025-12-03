using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Dtos
{
    public record ProjectSettingDto
    {
        public Guid Id { get; init; }
        public bool EnableKanban { get; init; }
        public bool EnableTimeTracking { get; init; }
        public bool EnableReports { get; init; }
        public bool ShowTeamMembers { get; init; }
        public bool SendEmailNotifications { get; init; }
        public string? CustomFields { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
    }
}
