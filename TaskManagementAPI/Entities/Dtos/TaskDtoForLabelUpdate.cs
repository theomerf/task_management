using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record TaskDtoForLabelUpdate
    {
        [Required(ErrorMessage = "Id alanı gereklidir.")]
        public Guid Id { get; init; }
        [Required(ErrorMessage = "Etiket id alanı gereklidir.")]
        public Guid LabelId { get; init; }
    }
}
