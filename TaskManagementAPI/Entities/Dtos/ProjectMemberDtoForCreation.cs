using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record ProjectMemberDtoForCreation
    {
        [Required(ErrorMessage = "E-Posta adresi gereklidir.")]
        public string Email { get; set; } = null!;
        public ProjectMemberRole Role { get; init; } = ProjectMemberRole.Member;
    }
}
