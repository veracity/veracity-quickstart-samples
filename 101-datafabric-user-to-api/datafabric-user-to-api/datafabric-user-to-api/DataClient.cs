using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace datafabric_user_to_api
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


        public async Task<DataFabricProfile> MyProfile()
        {
            var request = GetRequestMessageWithAuthorizationHeadersSet();
            request.RequestUri = new Uri(_options.BaseUrl + "users/me");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await DeserializeResponse<DataFabricProfile>(response);
        }

        public async Task<List<DataFabricContainer>> GetContainersSharedWithMe()
        {
            var request = GetRequestMessageWithAuthorizationHeadersSet();
            request.RequestUri = new Uri(_options.BaseUrl + "resources");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await DeserializeResponse<List<DataFabricContainer>>(response);
        }

        private HttpRequestMessage GetRequestMessageWithAuthorizationHeadersSet()
        {
            var token = _options.AccessToken;
            var subscriptionKey = _options.SubscriptionKey;
            var request = new HttpRequestMessage(HttpMethod.Get, "");
            request.Headers.Add("Authorization", token);
            request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            return request;
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