using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record TaskDtoForUpdate
    {
        [Required(ErrorMessage = "Id alanı gereklidir.")]
        public Guid Id { get; set; }
        public bool NotAssigned { get; set; } = false;
        public string? AssignedToId { get; set; }
        public Guid? LabelId { get; init; }
        [MinLength(5, ErrorMessage = "Açıklama en az 5 karakter olmalıdır.")]
        [MaxLength(2000, ErrorMessage = "Açıklama en fazla 2000 karakter olabilir.")]
        public string? Description { get; init; }
        [Required(ErrorMessage = "Başlık gereklidir.")]
        [MinLength(3, ErrorMessage = "Başlık en az 3 karakter olmalıdır.")]
        [MaxLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        public string Title { get; init; } = null!;
        public Entities.Models.TaskStatus Status { get; init; } = Entities.Models.TaskStatus.ToDo;
        public TaskPriority Priority { get; init; } = TaskPriority.Medium;
        public DateTime? StartDate { get; init; }
        public DateTime? DueDate { get; init; }
        public DateTime? CompletedAt { get; set; }
    }
}
