using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record AccountDtoForLogin
    {
        [Required(ErrorMessage = "Email gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçersiz email formatı.")]
        public string Email { get; init; } = null!;
        [Required(ErrorMessage = "Şifre gereklidir.")]
        [MinLength(6, ErrorMessage = "Şifre minimum 6 karakter olmalıdır.")]
        public string Password { get; init; } = null!;
        public bool RememberMe { get; init; } = false;
    }
}
