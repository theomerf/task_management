namespace Entities.Dtos
{
    public record TokenDto
    {
        public String AccessToken { get; set; } = null!;
        public String RefreshToken { get; set; } = null!;
    }
}
