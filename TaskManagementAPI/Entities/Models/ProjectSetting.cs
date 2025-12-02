using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class ProjectSetting
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProjectSettingSequence { get; set; }
        public long? ProjectId { get; set; }
        public bool EnableKanban { get; set; } = true;
        public bool EnableTimeTracking { get; set; } = true;
        public bool EnableReports { get; set; } = true;
        public bool ShowTeamMembers { get; set; } = true;
        public bool SendEmailNotifications { get; set; } = true;
        public string? CustomFields { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Project? Project { get; set; }
    }
}
