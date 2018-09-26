namespace veracity_api_sample.Models
{
    public class ContainerInput
    {
        /// <summary>
        /// The Location which a storage container will be provisioned. Containers can only be created in supported regions
        /// </summary>

        public string StorageLocation { get; set; }

        /// <summary>
        /// 5-32 character short name used to distinguish between storage containers. The name needs to be lowercase and alphanumeric. The full name of the container will comprise of this shortname plus a unique Guid genarated by the system. Note - storage containers can not be renamed
        /// </summary>
        public string ContainerShortName { get; set; }

        /// <summary>
        /// Indicates whether the user has accepted that the container will not contain personal data. Required to be true for a user to upload a container
        /// </summary>
        public bool MayContainPersonalData { get; set; }
    }
}