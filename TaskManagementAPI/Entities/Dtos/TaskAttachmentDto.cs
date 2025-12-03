namespace Entities.Dtos
{
    public record TaskAttachmentDto
    {
        public Guid Id { get; init; }
        public string FileUrl { get; init; } = null!;
        public string? ThumbnailUrl { get; init; }
        public DateTime UploadedAt { get; init; }
        public string UploadedByEmail { get; init; } = null!;
    }
}
