using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record TimeLogDtoForCreation
    {
        [Required(ErrorMessage = "Görev id'si gereklidir.")]
        public Guid TaskId { get; set; }
        public string? LoggedById { get; set; }
        public Guid? TimeLogCategoryId { get; set; }
        [MinLength(5, ErrorMessage = "Notlar en az 5 karakter olmalıdır.")]
        [MaxLength(2000, ErrorMessage = "Notlar en fazla 2000 karakter olabilir.")]
        public string? Notes { get; set; }
    }

    public record TimeLogCategoryDtoForCreation
    {
        [Required(ErrorMessage = "Kategori adı gereklidir.")]
        [MinLength(3, ErrorMessage = "Kategori adı en az 3 karakter olmalıdır.")]
        [MaxLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir.")]
        public string Name { get; init; } = null!;
        [MinLength(7, ErrorMessage = "Renk kodu 7 karakter olmalıdır.")]
        [MaxLength(7, ErrorMessage = "Renk kodu 7 karakter olmalıdır.")]
        public string Color { get; init; } = "#3B82F6";
    }
}
