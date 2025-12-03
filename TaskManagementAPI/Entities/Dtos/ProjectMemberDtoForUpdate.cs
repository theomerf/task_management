using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record ProjectMemberDtoForUpdate
    {
        [Required(ErrorMessage = "Id alanı gereklidir.")]
        public Guid Id { get; init; }
        public ProjectMemberRole Role { get; init; } = ProjectMemberRole.Member;
    }
}
