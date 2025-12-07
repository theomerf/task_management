using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record CommentDtoForCreation
    {
        public Guid? ParentCommentId { get; init; }
        [Required(ErrorMessage = "Yorum içeriği gereklidir.")]
        [MinLength(3, ErrorMessage = "Yorum içeriği en az 3 karakter uzunluğunda olmalıdır.")]
        [MaxLength(5000, ErrorMessage = "Yorum içeriği en fazla 5000 karakter uzunluğunda olabilir.")]
        public string Content { get; set; } = null!;
    }
}
