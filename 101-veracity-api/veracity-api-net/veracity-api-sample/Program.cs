using System;
using System.Threading.Tasks;

namespace veracity_api_sample
{
    /// <summary>
    /// Test class showing step by step process of using Data and Provisioning API
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // Subscription key provide by Veracity portal
            const string subscriptionKey = "< subscription key goes here >";
            // Base url to Data API service
            const string baseDataApiUrl = "https://< base ulr goes here >/DataAPI/api/";
            // Base url to Provisioning API service
            const string baseProvisioningApiUrl = "https://< base url goes here >/provisioningAPI/api/";

            // sign with AAD B2C authentication to get bearer key
            var signTask = AdB2C.SignIn();
            signTask.Wait();
            var signTaskResult = signTask.Result;
            if (signTaskResult == null)
            {
                Console.WriteLine("An error occured while requesting access token. Terminating.");
                Console.ReadLine();
                return;
            }
            var bearerKey = signTaskResult.AccessToken;
            // if you don't have any container created yet, below commented code is for you.
            // notice that provisioning of container can take up to 15 minutes so all next code will fail until container is created.
            //var provisionResults = CreateContainer(bearerKey, subscriptionKey, baseProvisioningApiUrl, <location from StorageLocations>, <container name, optional>);
            //provisionResults.Wait();

            // if container is created below code is for you.
            // if you don't have container yet, below code will return errors
            var results = ShareContainerUsingApi(bearerKey, subscriptionKey, baseDataApiUrl);
            results.Wait();
            // Hold the console
            Console.ReadLine();
        }
        /// <summary>
        /// Creates container in given location with given name (optional)
        /// Notice that provisioning of container can take up to 15 minutes
        /// </summary>
        /// <param name="bearerKey">authentication key. - for now pass through constructor.</param>
        /// <param name="subscriptionKey">user authentication key from Veracity portal</param>
        /// <param name="baseProvisioningApiUrl">url to Veracity ProvisioningAPI</param>
        /// <param name="location">StoraLocation where container will be stored</param>
        /// <param name="containerName">name of container, optional</param>
        /// <returns></returns>
        static async Task CreateContainer(string bearerKey, string subscriptionKey, string baseProvisioningApiUrl,
            StorageLocations location, string containerName = null)
        {
            var provisioningApi = new ProvisioningApi(bearerKey, subscriptionKey, baseProvisioningApiUrl);
            var provision = await provisioningApi.ProvisionContainer(location, containerName);
            Console.WriteLine(provision);
        }
        /// <summary>
        /// Shares container with current user.
        /// </summary>
        /// <param name="bearerKey">authentication key. - for now pass through constructor.</param>
        /// <param name="subscriptionKey">user authentication key from Veracity portal</param>
        /// <param name="baseDataApiUrl">url to Veracity DataAPI</param>
        /// <returns></returns>
        static async Task ShareContainerUsingApi(string bearerKey, string subscriptionKey, string baseDataApiUrl)
        {
            var api = new DataApi(bearerKey, subscriptionKey, baseDataApiUrl);
            // get current user
            var currentUser = await api.RequestCurrentUser();
            if (currentUser.Item2 == null)
            {
                Console.WriteLine($"Could not get current user. Error: {currentUser.Item1}");
                return;
            }
            Console.WriteLine($"Current user id: {currentUser.Item2.UserId}, company id: {currentUser.Item2.CompanyId}, role: {currentUser.Item2.Role}");

            var userId = currentUser.Item2.UserId; // needed for sharing

            // get storage templates. Its needed later on to correctly share resource.
            var templates = await api.RequestStorageKeyTemplates();
            if (templates.Item2 == null)
            {
                Console.WriteLine($"Could not get storage key templates. Error: {templates.Item1}");
                return;
            }
            if (templates.Item2.Count == 0)
            {
                Console.WriteLine("Storage key templates collection is empty!");
                return;
            }
            Console.WriteLine($"Storage template: {templates.Item2[0].Description}");

            var templateId = templates.Item2[0].Id; // needed for sharing

            // get all user resources. There should be at least one resource available.
            var resources = await api.RequestAllResources(true, true);
            if (resources.Item2 == null)
            {
                Console.WriteLine($"Could not get user resources. Error: {resources.Item1}");
                return;
            }
            if (resources.Item2.OwnedResources.Count == 0)
            {
                Console.WriteLine(
                    "Could not find any resource. Please create one using ProvisioningAPI. Notice that provisioning of container can take up to 15 minutes.");
                return;
            }
            Console.WriteLine($"Owned resource: {resources.Item2.OwnedResources[0]}");

            var resourceToShare = resources.Item2.OwnedResources[0].ResourceId;

            // share access to found resource with current user.
            var share = await api.ShareAccess(resourceToShare, true, userId, templateId);
            if (share.Item2 == null)
            {
                Console.WriteLine($"Could not share resource. Error: {share.Item1}");
                return;
            }
            Console.WriteLine($"Sharing id: {share.Item2.AccessSharingId}");

            var sharingId = share.Item2.AccessSharingId; // needed for fetching sas key.

            // fetch key to get SAS token
            var fetch = await api.FetchKeyForStorageContainer(resourceToShare, sharingId);
            if (fetch.Item2 == null)
            {
                Console.WriteLine($"Could not fetch key for resource. Error: {fetch.Item1}");
                return;
            }
            Console.WriteLine($"SAS uri: {fetch.Item2.SasUri}, SAS key: {fetch.Item2.SasKey}");

            // list all accesses.
            var accesses = await api.RequestAccesses(resourceToShare, 1, 1);
            if (accesses.Item2 == null)
            {
                Console.WriteLine($"Could not get accessess for resource. Error: {accesses.Item1}");
                return;
            }
            Console.WriteLine("Accesses for resource:");
            foreach (var access in accesses.Item2.Results)
                Console.WriteLine($"Resource id: {access.ResourceId}, OwnerId: {access.OwnerId}, UserId: {access.UserId}");
        }
    }
}