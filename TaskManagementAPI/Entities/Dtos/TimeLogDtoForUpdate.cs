using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record TimeLogDtoForUpdate
    {
        [Required(ErrorMessage = "Id alanı gereklidir.")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Görev Id alanı gereklidir.")]
        public Guid TaskId { get; set; }
        public Guid? TimeLogCategoryId { get; set; }
        [MinLength(5, ErrorMessage = "Notlar en az 5 karakter olmalıdır.")]
        [MaxLength(2000, ErrorMessage = "Notlar en fazla 2000 karakter olabilir.")]
        public string? Notes { get; set; }
    }

    public record TimeLogCategoryDtoForUpdate : TimeLogCategoryDtoForCreation
    {
        [Required(ErrorMessage = "Id alanı gereklidir.")]
        public Guid Id { get; set; }
    }
}
