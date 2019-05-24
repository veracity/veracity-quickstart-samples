using System;

namespace datafabric_sharing_container
{
    public class DataFabricContainer
    {
        public string id { get; set; }
        public string reference { get; set; }
        public string url { get; set; }
        public DateTime lastModifiedUTC { get; set; }
        public DateTime creationDateTimeUTC { get; set; }
        public string ownerId { get; set; }
        public string accessLevel { get; set; }
        public string region { get; set; }
        public string keyStatus { get; set; }
        public string mayContainPersonalData { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Metadata
    {
        public string title { get; set; }
        public string description { get; set; }
        public Icon icon { get; set; }
        public Tag[] tags { get; set; }
    }

    public class Icon
    {
        public string id { get; set; }
        public string backgroundColor { get; set; }
    }

    public class Tag
    {
        public string id { get; set; }
        public string title { get; set; }
    }

}
