using Entities.Models;
using System.Xml.Linq;

namespace Entities.RequestFeatures
{
    public class TaskRequestParameters
    {
        public string? Title { get; set; }
        public Entities.Models.TaskStatus? Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public long? LabelId { get; set; }
        public bool? Me { get; set; }
        public string? SortBy { get; set; }

        public void Validate()
        {
            if (!string.IsNullOrWhiteSpace(Title) && Title.Length < 2)
                Title = null;
        }
    }
}
