using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Account : IdentityUser
    {
        [NotMapped]
        public override string? UserName
        {
            get => Email;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Email = value;
                }
            }

        }
        public string? AvatarUrl { get; set; } = "https://i.hizliresim.com/ntfecdo.png";
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime MembershipDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginDate { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime? DeletedAt { get; set; }
        [NotMapped]
        public bool IsDeleted => DeletedAt.HasValue;
        public ICollection<Project> CreatedProjects { get; set; } = new List<Project>();
        public ICollection<Task> CreatedTasks { get; set; } = new List<Task>();
        public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
        public ICollection<ProjectMember> ProjectMemberships { get; set; } = new List<ProjectMember>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    }
}
