using System;
using veracity_api_sample.Enums;

namespace veracity_api_sample.Models
{
    /// <summary>
    /// Json structure for single Resource
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// Container ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The name of the container in Azure. 
        /// <example>
        /// my-container5e1b021a-d3dc-4cdd-ba1e-7399db38ecf4
        /// </example>
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// The full container url in Azure. 
        /// <example>
        /// https://ne1dnvgltstgcus0000f.blob.core.windows.net/my-container5e1b021a-d3dc-4cdd-ba1e-7399db38ecf4
        /// </example>
        /// </summary>
        public string Url { get; set; }

        public DateTime LastModifiedUTC { get; set; }
        public Guid OwnerId { get; set; }
        public AccessLevel AccessLevel { get; set; }


        /// <summary>
        /// Which region the resource was created in. Valid values: "USA" | "Europe"
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// <see cref="KeyStatus"/>
        /// </summary>
        public KeyStatus KeyStatus { get; set; }
    }
}