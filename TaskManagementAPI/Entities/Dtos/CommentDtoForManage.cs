using Entities.Models;

namespace Entities.Dtos
{
    public record CommentDtoForManage
    {
        public Comment Comment { get; init; } = null!;
        public string AuthorId { get; init; } = null!;
        public long TaskId { get; init; }
        public long ProjectSequence { get; init; }
        public ProjectStatus ProjectStatus { get; init; }
        public string ProjectCreatedById { get; init; } = null!;
    }
}
