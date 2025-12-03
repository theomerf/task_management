using Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Dtos
{
    public record ProjectMemberDto
    {
        public Guid Id { get; init; }
        public string? AccountId { get; init; }
        public ProjectMemberRole Role { get; init; }
        public DateTime JoinedAt { get; init; }
        public DateTime? LeftAt { get; init; }
        [NotMapped]
        public bool IsActive => !LeftAt.HasValue;
        public string AccountFirstName { get; init; } = null!;
        public string AccountLastName { get; init; } = null!;
        public string AccountEmail { get; init; } = null!;
    }
}
