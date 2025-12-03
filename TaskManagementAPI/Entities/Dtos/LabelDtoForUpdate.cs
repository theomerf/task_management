using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record LabelDtoForUpdate : LabelDtoForCreation
    {
        [Required(ErrorMessage = "Id alanı gereklidir.")]
        public Guid Id { get; init; }
    }
}
