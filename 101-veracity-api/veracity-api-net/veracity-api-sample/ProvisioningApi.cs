using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using veracity_api_sample.Enums;
using veracity_api_sample.Models;

namespace veracity_api_sample
{
    /// <summary>
    /// Sample class showing Veracity Provisioning API usage
    /// </summary>
    public class ProvisioningApi
    {
        private readonly string _baseProvisioningApiUrl;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor taking bearerKey, subscriptionKey and baseUrl to provisioningAPI
        /// </summary>
        /// <param name="bearerKey">authentication key. - for now pass through constructor.</param>
        /// <param name="subscriptionKey">user authentication key from Veracity portal</param>
        /// <param name="baseProvisioningApiUrl">url to Veracity ProvisioningAPI</param>
        public ProvisioningApi(string bearerKey, string subscriptionKey, string baseProvisioningApiUrl)
        {
            _baseProvisioningApiUrl = baseProvisioningApiUrl;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerKey);
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
        }

        /// <summary>
        /// Creates new storage container in given location with optionally given name
        /// </summary>
        /// <param name="storageLocation">location of storage container</param>
        /// <param name="containerShortName">container name, optional</param>
        /// <returns>Error if operation failed, Accepted - when success</returns>
        public async Task<string> ProvisionContainer(StorageLocations storageLocation, string containerShortName = null)
        {
            var uri = $"{_baseProvisioningApiUrl}container";

            var containerInput = new ContainerInput
            {
                ContainerShortName = containerShortName,
                MayContainPersonalData = false,
                StorageLocation = storageLocation.ToString()
            };

            var response = await _httpClient.PostAsync(uri, new StringContent(JsonConvert.SerializeObject(containerInput), Encoding.UTF8, "application/json"));
            var responseContent = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode ? response.ReasonPhrase : responseContent;
        }
    }
}