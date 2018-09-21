namespace veracity_api_sample.Models
{
    /// <summary>
    /// Json structure for single Access data
    /// </summary>
    public class Access
    {
        public string ProviderEmail { get; set; }
        public string UserId { get; set; }
        public string OwnerId { get; set; }
        public string AccessSharingId { get; set; }
        public bool KeyCreated { get; set; }
        public bool AutoRefreshed { get; set; }
        public string KeyCreatedTimeUTC { get; set; }
        public string KeyExpiryTimeUTC { get; set; }
        public string ResourceType { get; set; }
        public int AccessHours { get; set; }
        public string AccessKeyTemplateId { get; set; }
        public bool Attribute1 { get; set; }
        public bool Attribute2 { get; set; }
        public bool Attribute3 { get; set; }
        public bool Attribute4 { get; set; }
        public string ResourceId { get; set; }
    }
}