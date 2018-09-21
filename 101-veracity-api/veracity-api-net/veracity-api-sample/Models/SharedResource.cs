namespace veracity_api_sample.Models
{
    /// <summary>
    /// Json structure for Shared Resource
    /// </summary>
    public class SharedResource
    {
        public Resource StorageItem { get; set; }
        public string AccessDescription { get; set; }
        public bool AccessKeyCreated { get; set; }
        public string AccessKeyEndDateUTC { get; set; }
        public string AccessKeyTemplateId { get; set; }
        public string AccessSharingId { get; set; }
        public bool AutoRefreshed { get; set; }
    }
}