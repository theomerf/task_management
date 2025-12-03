using Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record LabelDtoForCreation
    {
        public long? ProjectId { get; set; }
        [Required(ErrorMessage = "Etiket adı gereklidir.")]
        [MinLength(3, ErrorMessage = "Etiket adı en az 3 karakter uzunluğunda olmalıdır.")]
        [MaxLength(100, ErrorMessage = "Etiket adı en fazla 100 karakter uzunluğunda olabilir.")]
        public string Name { get; init; } = null!;
        [MinLength(7, ErrorMessage = "Etiket rengi 7 karakter uzunluğunda olmalıdır.")]
        [MaxLength(7, ErrorMessage = "Etiket rengi 7 karakter uzunluğunda olmalıdır.")]
        public string Color { get; init; } = "#3B82F6";
        [MinLength(3, ErrorMessage = "Etiket açıklaması en az 3 karakter uzunluğunda olmalıdır.")]
        [MaxLength(500, ErrorMessage = "Etiket açıklaması en fazla 500 karakter uzunluğunda olabilir.")]
        public string? Description { get; init; }
    }
}
