using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record ProjectDtoForCreation
    {
        public string? CreatedById { get; set; }
        [Required(ErrorMessage = "Proje adı gereklidir.")]
        [MinLength(3, ErrorMessage = "Proje adı en az 3 karakter olmalıdır.")]
        [MaxLength(200, ErrorMessage = "Proje adı en fazla 200 karakter olabilir.")]
        public string Name { get; set; } = null!;
        [MaxLength(2000, ErrorMessage = "Proje açıklaması en fazla 2000 karakter olabilir.")]
        public string? Description { get; set; }
        [MaxLength(10, ErrorMessage = "Proje simgesi en fazla 10 karakter olabilir.")]
        public string? Icon { get; set; }
        [MinLength(7, ErrorMessage = "Proje rengi 7 karakter olmalıdır.")]
        [MaxLength(7, ErrorMessage = "Proje rengi 7 karakter olmalıdır.")]
        public string? Color { get; set; }
        public ProjectStatus? Status { get; set; }
        public ProjectVisibility? Visibility { get; set; }
        public ProjectSettingDtoForCreation? ProjectSetting { get; set; }
    }
}
