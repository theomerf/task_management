using Entities.Models;

namespace Entities.Dtos
{
    public record ProjectMemberDtoForCreation
    {
        public long? ProjectId { get; set; }
        public string? AccountId { get; set; }
        public ProjectMemberRole Role { get; init; } = ProjectMemberRole.Member;
    }
}
