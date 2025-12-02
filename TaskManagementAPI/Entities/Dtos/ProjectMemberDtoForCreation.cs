using Entities.Models;

namespace Entities.Dtos
{
    public record ProjectMemberDtoForCreation
    {
        public long? ProjectId { get; set; }
        public string AccountId { get; init; } = null!;
        public ProjectMemberRole? Role { get; init; }
    }
}
