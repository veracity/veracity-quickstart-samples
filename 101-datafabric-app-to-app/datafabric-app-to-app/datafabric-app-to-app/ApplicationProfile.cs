using System;

namespace datafabric_app_to_app
{
    /// <summary>
    /// Object model of the response from data api for our application profile
    /// </summary>
    public class ApplicationProfile
    {
        public Guid id { get; set; }
        public Guid companyId { get; set; }
        public string role { get; set; }
    }

}
