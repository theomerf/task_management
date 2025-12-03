using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record ProjectDtoForUpdate
    {
        [Required(ErrorMessage = "Id alanı gereklidir.")]
        public Guid Id { get; init; }
        public string? CreatedById { get; set; }
        [Required(ErrorMessage = "Proje adı gereklidir.")]
        [MinLength(3, ErrorMessage = "Proje adı en az 3 karakter olmalıdır.")]
        [MaxLength(200, ErrorMessage = "Proje adı en fazla 200 karakter olabilir.")]
        public string Name { get; init; } = null!;
        [MaxLength(2000, ErrorMessage = "Proje açıklaması en fazla 2000 karakter olabilir.")]
        public string? Description { get; init; }
        [MaxLength(10, ErrorMessage = "Proje simgesi en fazla 10 karakter olabilir.")]
        public string? Icon { get; init; }
        [MinLength(7, ErrorMessage = "Proje rengi 7 karakter olmalıdır.")]
        [MaxLength(7, ErrorMessage = "Proje rengi 7 karakter olmalıdır.")]
        public string? Color { get; init; }
        public ProjectStatus? Status { get; init; }
        public ProjectVisibility? Visibility { get; init; }
    }
}
