using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public class MentionDtoForCreation
    {
        [Required(ErrorMessage = "Bahsedilen kullanıcı id'si gereklidir.")]
        public string MentionedUserId { get; init; } = null!;
    }
}
