namespace Entities.Dtos
{
    public record MentionDto
    {
        public Guid Id { get; set; }
        public bool IsRead { get; set; }
        public string MentionedUserEmail { get; init; } = null!;
        public string MentionedUserFirstName { get; init; } = null!;
        public string MentionedUserLastName { get; init; } = null!;
    }
}
