namespace Entities.Dtos
{
    public record LoginResponseDto
    {
        public String Email { get; init; } = null!;
        public String FirstName { get; init; } = null!;
        public String LastName { get; init; } = null!;
    }
}
