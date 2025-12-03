using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record TaskAttachmentDtoForCreation
    {
        [Required(ErrorMessage = "Görev id'si gereklidir.")]
        public Guid TaskId { get; init; }
        public string? UploadedById { get; init; }
        [MinLength(1, ErrorMessage = "Dosya adı en az 1 karakter olmalıdır.")]
        [MaxLength(255, ErrorMessage = "Dosya adı en fazla 255 karakter olabilir.")]
        public string? FileName { get; init; }
        [MaxLength(255, ErrorMessage = "Dosya url'si en fazla 255 karakter olabilir.")]
        public string FileUrl { get; init; } = null!;
        [MaxLength(255, ErrorMessage = "Thumbnail url'si en fazla 255 karakter olabilir.")]
        public string? ThumbnailUrl { get; init; }
    }
}
