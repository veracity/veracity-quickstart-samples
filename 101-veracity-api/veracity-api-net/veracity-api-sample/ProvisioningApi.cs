using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace veracity_api_sample
{
    /// <summary>
    /// Only those can be choosen when considering Storage Locations
    /// </summary>
    public enum StorageLocations
    {
        Unknown,
        NorthEurope,
        EastUs1
    }
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
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            if(!string.IsNullOrEmpty(containerShortName))
                queryString["containerShortName"] = containerShortName;

            var requestCode = Guid.NewGuid().ToString();

            var uri = $"{_baseProvisioningApiUrl}container?storageLocation={storageLocation}&requestCode={requestCode}&{queryString}";
            var body = JsonConvert.SerializeObject(new { StorageLocation = storageLocation.ToString(), RequestCode = requestCode, ContainerShortName = containerShortName });

            var response = await _httpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json"));
            var responseContent = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode ? response.ReasonPhrase : responseContent;
        }
    }
}