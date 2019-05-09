using System;

namespace datafabric_app_to_app
{
    public class DataClientOptions
    {
        // These values are provided to you when your application is onboarded
        public string AadTenantId { get; set; }
        public string ResourceUrl { get; set; }
        public string ClientId { get; set; }

        // This is a secret, and should be treated as such, never check this into source control, access to this should be restricted and stored in a secure location (like azure keyvault)
        public string ClientSecret { get; set; }
        public Environment Envrionment { get; set; }


        /// <summary>
        /// Set this in configuration.json, you will have to update the other values in the json file as well to match the environment of the API you are calling
        /// </summary>
        public enum Environment
        {
            Test,
            Staging,
            Production
        }

        public string BaseUrl
        {
            get
            {
                switch (Envrionment)
                {
                    case Environment.Test:
                        return "https://api-test.veracity.com/veracity/datafabric/data/api/1/";
                    case Environment.Staging:
                        return "https://api-stag.veracity.com/veracity/datafabric/data/api/1/";
                    case Environment.Production:
                        return "https://api.veracity.com/veracity/datafabric/data/api/1/";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public string SubscriptionKey { get; set; }
    }
}
