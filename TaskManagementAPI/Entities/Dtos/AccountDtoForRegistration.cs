using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record AccountDtoForRegistration
    {
        [Required(ErrorMessage = "Ad gereklidir.")]
        [MinLength(2, ErrorMessage = "Ad minimum 2 karakter olmalıdır.")]
        [MaxLength(100, ErrorMessage = "Ad en fazla 100 karakter olmalıdır.")]
        public string FirstName { get; init; } = null!;
        [MinLength(2, ErrorMessage = "Soyad minimum 2 karakter olmalıdır.")]
        [MaxLength(100, ErrorMessage = "Soyad en fazla 100 karakter olmalıdır.")]
        [Required(ErrorMessage = "Soyad gereklidir.")]
        public string LastName { get; init; } = null!;
        [Required(ErrorMessage = "Şifre gereklidir.")]
        [MinLength(6, ErrorMessage = "Şifre minimum 6 karakter olmalıdır.")]
        [MaxLength(20, ErrorMessage = "Şifre en fazla 20 karakter olmalıdır.")]
        public string Password { get; init; } = null!;
        [EmailAddress(ErrorMessage = "Geçersiz email formatı.")]
        [Required(ErrorMessage = "Email gereklidir.")]
        public string Email { get; init; } = null!;
        [Required(ErrorMessage = "Telefon numarası gereklidir.")]
        public string PhoneNumber { get; init; } = null!;
        public ICollection<string> Roles { get; init; } = ["Member"];
    }
}
