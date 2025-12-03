using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProjectSequence { get; set; }
        public string? CreatedById { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Icon { get; set; } = "📁";
        public string? Color { get; set; } = "#3B82F6";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;
        public ProjectVisibility Visibility { get; set; } = ProjectVisibility.Private;
        public int TaskCount { get; set; } = 0;
        public int CompletedTaskCount { get; set; } = 0;
        public DateTime? DeletedAt { get; set; }
        [NotMapped]
        public bool IsDeleted => DeletedAt.HasValue;
        public Account? CreatedBy { get; set; }
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
        public ICollection<Label> Labels { get; set; } = new List<Label>();
        public ICollection<TimeLogCategory> TimeLogCategories { get; set; } = new List<TimeLogCategory>();
        public ProjectSetting Settings { get; set; } = null!;
        public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    }

    public enum ProjectStatus
    {
        Active = 0,
        Archived = 1,
        Completed = 2,
        OnHold = 3
    }

    public enum ProjectVisibility
    {
        Private = 0,
        Public = 1,
        Team = 2,
    }
}
