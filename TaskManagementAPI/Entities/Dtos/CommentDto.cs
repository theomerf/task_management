namespace Entities.Dtos
{
    public record CommentDto
    {
        public Guid Id { get; init; }
        public string AuthorEmail { get; init; } = null!;
        public string AuthorFirstName { get; init; } = null!;
        public string AuthorLastName { get; init; } = null!;
        public string Content { get; init; } = null!;
        public DateTime CreatedAt { get; init; }
        public string? Reactions { get; init; }
    }
}
