namespace veracity_api_sample.Models
{
    /// <summary>
    /// Json structure for StorageKeyTemplate
    /// </summary>
    public class StorageKeyTemplate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int TotalHours { get; set; }
        public bool IsSystemKey { get; set; }
        public string Description { get; set; }
        public bool Attribute1 { get; set; }
        public bool Attribute2 { get; set; }
        public bool Attribute3 { get; set; }
        public bool Attribute4 { get; set; }
    }
}