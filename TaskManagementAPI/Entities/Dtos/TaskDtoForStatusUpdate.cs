using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record TaskDtoForStatusUpdate
    {
        [Required(ErrorMessage = "Id alanı gereklidir.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Durum bilgisi gereklidir.")]
        public Entities.Models.TaskStatus Status { get; init; } = Entities.Models.TaskStatus.ToDo;
    }
}
