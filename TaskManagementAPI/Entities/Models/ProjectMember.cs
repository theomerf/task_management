using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class ProjectMember
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProjectMemberSequence { get; set; } 
        public long ProjectId { get; set; }
        public string? AccountId { get; set; }
        public ProjectMemberRole Role { get; set; } = ProjectMemberRole.Member;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LeftAt { get; set; }
        [NotMapped]
        public bool IsActive => !LeftAt.HasValue;
        public Project? Project { get; set; }
        public Account? Account { get; set; }
    }

    public enum ProjectMemberRole
    {
        Owner = 0,
        Manager = 1,
        Member = 2
    }
}
