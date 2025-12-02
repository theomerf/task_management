using Entities.Models;

namespace Entities.RequestFeatures
{
    public class ProjectRequestParameters
    {
        public string? Name { get; set; }
        public ProjectStatus? Status { get; set; }
        public ProjectVisibility? Visibility { get; set; }
        public ProjectMemberRole? MemberRole { get; set; }
        public string? SortBy { get; set; }

        public void Validate()
        {
            if (!string.IsNullOrWhiteSpace(Name) && Name.Length < 2)
                Name = null;
        }
    }

    public class ProjectRequestParametersForAdmin : RequestParameters
    {
        public string? Name { get; set; }
        public ProjectStatus? Status { get; set; }
        public ProjectVisibility? Visibility { get; set; }
        public string? SortBy { get; set; }
        public bool? IsDeleted { get; set; }

        public override void Validate()
        {
            base.Validate();

            if (!string.IsNullOrWhiteSpace(Name) && Name.Length < 2)
                Name = null;
        }
    }
}
