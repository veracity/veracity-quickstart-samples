using Newtonsoft.Json;

namespace datafabric_app_to_app
{
    /// <summary>
    /// Object model for response from Oauth token provider on login
    /// </summary>
    public class B2CAccessToken
    {
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresInMinutes { get; set; }
        [JsonProperty(PropertyName = "ext_expires_in")]
        public int ExtExpiresInMinutes { get; set; }
        [JsonProperty(PropertyName = "expires_on")]
        public int ExpiresOnTimeStamp { get; set; }
        [JsonProperty(PropertyName = "not_before")]
        public int ExpiresNotBeforeTimeStamp { get; set; }
        [JsonProperty(PropertyName = "resource")]
        public string Resource { get; set; }
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

    }
}
