using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record ProjectSettingDtoForUpdate : ProjectSettingDtoForCreation
    {
        [Required(ErrorMessage = "Id alanı gereklidir.")]
        public Guid Id { get; set; }
    }
}
