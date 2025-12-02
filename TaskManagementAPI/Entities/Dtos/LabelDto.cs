namespace Entities.Dtos
{
    public record LabelDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string Color { get; init; } = null!;
        public string? Description { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
