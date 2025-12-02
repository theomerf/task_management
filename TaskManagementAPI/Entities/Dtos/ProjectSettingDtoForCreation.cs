using Entities.Models;

namespace Entities.Dtos
{
    public record ProjectSettingDtoForCreation
    {
        public long ProjectId { get; init; }
        public bool EnableKanban { get; init; } = true;
        public bool EnableTimeTracking { get; init; } = true;
        public bool EnableReports { get; init; } = true;
        public bool ShowTeamMembers { get; init; } = true;
        public bool SendEmailNotifications { get; init; } = true;
        public string? CustomFields { get; init; }
    }
}
