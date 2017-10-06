using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace veracity_api_sample
{
    /// <summary>
    /// Sample class showing Veracity Data API usage
    /// </summary>
    public class DataApi
    {
        private readonly string _baseDataApiUrl;
        private readonly HttpClient _httpClient;
        /// <summary>
        /// Constructor taking bearerKey, subscriptionKey and baseUrl to dataAPI
        /// </summary>
        /// <param name="bearerKey">authentication key. - for now pass through constructor.</param>
        /// <param name="subscriptionKey">user authentication key from Veracity portal</param>
        /// <param name="baseDataApiUrl">url to Veracity DataAPI</param>
        public DataApi(string bearerKey, string subscriptionKey, string baseDataApiUrl)
        {
            _baseDataApiUrl = baseDataApiUrl;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerKey);
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
        }
        /// <summary>
        /// Returns User infor about current user.
        /// </summary>
        /// <returns>Tuple of error string and User object. Error string is empty if operation is successfull.</returns>
        public async Task<Tuple<string, User>> RequestCurrentUser()
        {
            var uri = $"{_baseDataApiUrl}users/me?";

            var response = await _httpClient.GetAsync(uri);
            var responseContent = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode
                ? new Tuple<string, User>("", JsonConvert.DeserializeObject<User>(responseContent))
                : new Tuple<string, User>(responseContent, null);
        }
        /// <summary>
        /// Returns User info about user with specified ID.
        /// </summary>
        /// <param name="userId">id of the user to look for.</param>
        /// <returns>Tuple of error string and User object. Error string is empty if operation is successfull.</returns>
        public async Task<Tuple<string, User>> RequestUser(string userId)
        {
            var uri = $"{_baseDataApiUrl}users/{userId}";

            var response = await _httpClient.GetAsync(uri);
            var responseContent = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode
                ? new Tuple<string, User>("", JsonConvert.DeserializeObject<User>(responseContent))
                : new Tuple<string, User>(responseContent, null);
        }
        /// <summary>
        /// Returns Company infor about company with specified ID.
        /// </summary>
        /// <param name="companyId">id of the company to look for</param>
        /// <returns>Tuple of error string and Company object. Error string is empty if operation is successfull.</returns>
        public async Task<Tuple<string, Company>> RequestCompany(string companyId)
        {
            var uri = $"{_baseDataApiUrl}companies/{companyId}";

            var response = await _httpClient.GetAsync(uri);
            var responseContent = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode
                ? new Tuple<string, Company>("", JsonConvert.DeserializeObject<Company>(responseContent))
                : new Tuple<string, Company>(responseContent, null);
        }
        /// <summary>
        /// Returns collection of storage key templates with different priviliges
        /// </summary>
        /// <returns>Tuple of error string and templates collection. Error string is empty if operation is successfull.</returns>
        public async Task<Tuple<string, List<StorageKeyTemplate>>> RequestStorageKeyTemplates()
        {
            var uri = $"{_baseDataApiUrl}keytemplates";

            var response = await _httpClient.GetAsync(uri);
            var responseContent = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode
                ? new Tuple<string, List<StorageKeyTemplate>>("", JsonConvert.DeserializeObject<List<StorageKeyTemplate>>(responseContent))
                : new Tuple<string, List<StorageKeyTemplate>>(responseContent, null);
        }
        /// <summary>
        /// Returns collection of resources.
        /// </summary>
        /// <param name="shared">true if shared resources should be included in result collection</param>
        /// <param name="owned">true if owned resources should be included in result collection</param>
        /// <returns>Tuple of error string and Resources object. Error string is empty if operation is successfull.</returns>
        public async Task<Tuple<string, Resources>> RequestAllResources(bool shared, bool owned)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["shared"] = shared.ToString();
            queryString["owned"] = owned.ToString();

            var uri = $"{_baseDataApiUrl}resources?{queryString}";

            var response = await _httpClient.GetAsync(uri);
            var responseContent = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode
                ? new Tuple<string, Resources>("", JsonConvert.DeserializeObject<Resources>(responseContent))
                : new Tuple<string, Resources>(responseContent, null);
        }
        /// <summary>
        /// Returns collection of current accesses to resource.
        /// </summary>
        /// <param name="resourceId">id of resource to look for</param>
        /// <param name="pageNo">page of results to return</param>
        /// <param name="pageSize">size of page</param>
        /// <returns>Tuple of error string and Accesses object. Error string is empty if operation is successfull.</returns>
        public async Task<Tuple<string, Accesses>> RequestAccesses(string resourceId, int pageNo, int pageSize)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["pageNo"] = pageNo.ToString();
            queryString["pageSize"] = pageSize.ToString();

            var uri = $"{_baseDataApiUrl}resources/{resourceId}/accesses?" + queryString;

            var response = await _httpClient.GetAsync(uri);
            var responseContent = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode
                ? new Tuple<string, Accesses>("", JsonConvert.DeserializeObject<Accesses>(responseContent))
                : new Tuple<string, Accesses>(responseContent, null);
        }
        /// <summary>
        /// Returns shareId of resource to be shared with given user and given rights
        /// </summary>
        /// <param name="resourceId">id of resource to be shared</param>
        /// <param name="autoRefreshed">true if share should refresh automatically</param>
        /// <param name="userToShareId">id of user to share resource with</param>
        /// <param name="shareTemplateId">id of template with proper rights</param>
        /// <returns>Tuple of error string and ShareAccessResponse object. Error string is empty if operation is successfull.</returns>
        public async Task<Tuple<string, ShareAccessResponse>> ShareAccess(string resourceId, bool autoRefreshed,
            string userToShareId, string shareTemplateId)
        {
            var uri = $"{_baseDataApiUrl}resources/{resourceId}/accesses?autoRefreshed={autoRefreshed}";
            var body = JsonConvert.SerializeObject(new { UserId = userToShareId, AccessKeyTemplateId = shareTemplateId });

            var response = await _httpClient.PostAsync(uri, new StringContent(body, Encoding.UTF8, "application/json"));
            var responseContent = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode
                ? new Tuple<string, ShareAccessResponse>("", JsonConvert.DeserializeObject<ShareAccessResponse>(responseContent))
                : new Tuple<string, ShareAccessResponse>(responseContent, null);
        }
        /// <summary>
        /// Returns SAS token info for given resource and sharingID
        /// </summary>
        /// <param name="resourceId">id of resource to look for</param>
        /// <param name="accessSharingId">sharing id for resource</param>
        /// <returns>Tuple of error string and SasData object. Error string is empty if operation is successfull.</returns>
        public async Task<Tuple<string, SasData>> FetchKeyForStorageContainer(string resourceId, string accessSharingId)
        {
            var uri = $"{_baseDataApiUrl}resources/{resourceId}/keys?accessSharingId={accessSharingId}";

            var response = await _httpClient.GetAsync(uri);
            var responseContent = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode
                ? new Tuple<string, SasData>("", JsonConvert.DeserializeObject<SasData>(responseContent))
                : new Tuple<string, SasData>(responseContent, null);
        }
    }
    /// <summary>
    /// Json structure for User
    /// </summary>
    public class User
    {
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string Role { get; set; }
    }
    /// <summary>
    /// Json structure for Company
    /// </summary>
    public class Company
    {
        public string CompanyId { get; set; }
        public string AzureId { get; set; }
    }
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
    /// <summary>
    /// Json structure for Resources. Owned and Shared
    /// </summary>
    public class Resources
    {
        public List<Resource> OwnedResources { get; set; }
        public List<SharedResource> SharedResources { get; set; }
    }
    /// <summary>
    /// Json structure for single Resource
    /// </summary>
    public class Resource
    {
        public string ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string ResourceUrl { get; set; }
        public string LastModifiedUTC { get; set; }
        public string OwnerId { get; set; }
        public string ConsumerName { get; set; }
        public string ResourceType { get; set; }
    }
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
    /// <summary>
    /// Json structure for Accesses result
    /// </summary>
    public class Accesses
    {
        public List<Access> Results { get; set; }
        public int Page { get; set; }
        public int ResultsPerPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
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
    /// <summary>
    /// Json structure for SAS token info data
    /// </summary>
    public class SasData
    {
        public string SasKey { get; set; }
        public string SasUri { get; set; }
        public string FillKey { get; set; }
        public string SasKeyExpiryTimeUTC { get; set; }
        public bool IsKeyExpired { get; set; }
        public bool AutoRefreshed { get; set; }
    }
    /// <summary>
    /// Json structure for shared access response
    /// </summary>
    public class ShareAccessResponse
    {
        public string AccessSharingId { get; set; }
    }
}