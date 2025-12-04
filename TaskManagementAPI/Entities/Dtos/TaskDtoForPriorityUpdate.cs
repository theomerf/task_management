using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record TaskDtoForPriorityUpdate
    {
        [Required(ErrorMessage = "Id alanı gereklidir.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Öncelik alanı gereklidir.")]
        public TaskPriority Priority { get; init; } = TaskPriority.Medium;
    }
}
