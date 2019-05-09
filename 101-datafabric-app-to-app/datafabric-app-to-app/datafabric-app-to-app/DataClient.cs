using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace datafabric_app_to_app
{
    /// <summary>
    /// A helper class that takes care of authenticated calls towards data fabric api
    /// </summary>
    public class DataClient
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly DataClientOptions _options;


        public DataClient(IOptions<DataClientOptions> options)
        {
            _options = options.Value;
        }


      


        public async Task<ApplicationProfile> MyApplicationId()
        {
            var uri = _options.BaseUrl + "application";
            var token = await AcquireToken();
            var subscriptionsKey = _options.SubscriptionKey;
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Authorization", "Bearer " + token.AccessToken);
            request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionsKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await DeserializeResponse<ApplicationProfile>(response);
        }

        public async Task<List<DataFabricContainer>> GetContainersSharedWithApp()
        {
            var request = await GetRequestMessageWithAuthorizationHeadersSet();
            request.RequestUri = new Uri(_options.BaseUrl + "resources");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await DeserializeResponse<List<DataFabricContainer>>(response);
        }

        private async Task<HttpRequestMessage> GetRequestMessageWithAuthorizationHeadersSet()
        {
            var token = await AcquireToken();
            var subscriptionKey = _options.SubscriptionKey;
            var request = new HttpRequestMessage(HttpMethod.Get, "");
            request.Headers.Add("Authorization", "Bearer " + token.AccessToken);
            request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            return request;
        }



        public async Task<B2CAccessToken> AcquireToken()
        {
            var tokenEndpoint = $"https://login.microsoftonline.com/{_options.AadTenantId}/oauth2/token";
            var requestParams = new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"},
                {"client_id", _options.ClientId},
                {"resource", _options.ResourceUrl},
                {"client_secret", _options.ClientSecret}
            };

            var requestBody = string.Join("&", requestParams.Select(x => x.Key + "=" + x.Value));
            var content = new StringContent(requestBody, Encoding.UTF8);
            var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint) { Content = content };

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await DeserializeResponse<B2CAccessToken>(response);
        }

        private static async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            using (var httpStream = await response.Content.ReadAsStreamAsync())
            using (var streamReader = new StreamReader(httpStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                var obj = serializer.Deserialize<T>(jsonReader);
                return obj;
            }
        }
    }
}