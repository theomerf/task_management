namespace Entities.RequestFeatures
{
    public class ActivityLogRequestParameters : RequestParameters
    {
        public string? AccountId { get; set; }
        public Guid? TaskId { get; set; }
        public int? ActivityType { get; set; }
    }
}
