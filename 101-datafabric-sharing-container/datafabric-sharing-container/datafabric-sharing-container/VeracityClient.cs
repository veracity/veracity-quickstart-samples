using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace datafabric_sharing_container
{
    public class VeracityClient
    {
        private readonly VeracityApiOptions _options;
        private static readonly HttpClient HttpClient = new HttpClient();

        protected VeracityClient(IOptions<VeracityApiOptions> options)
        {
            _options = options.Value;
        }


        protected async Task<T> SendAsync<T>(string to)
        {
            var request = GetRequestMessageWithAuthorizationHeadersSet();
            request.RequestUri = new Uri($"{_options.BaseUrl}{to}");

            return await SendAsync<T>(request);
        }


        protected async Task<T> SendAsync<T>(string to, object body)
        {
            var request = GetRequestMessageWithAuthorizationHeadersSet();
            request.RequestUri = new Uri($"{_options.BaseUrl}{to}");
            request.Method = HttpMethod.Post;
            var serialized = JsonConvert.SerializeObject(body);
            request.Content = new StringContent(serialized, Encoding.UTF8, "application/json");

            return await SendAsync<T>(request);
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

        private async Task<T> SendAsync<T>(HttpRequestMessage request)
        {
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await DeserializeResponse<T>(response);
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