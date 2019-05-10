using System;

namespace datafabric_user_to_api
{
    public class DataClientOptions
    {
        public string AccessToken { get; set; }
        public Environment Env { get; set; }

        public enum Environment
        {
            Undefined,
            Test,
            Staging,
            Production
        }

        public string BaseUrl
        {
            get
            {
                switch (Env)
                {
                    case Environment.Test:
                        return "https://api-test.veracity.com/veracity/datafabric/data/api/1/";
                    case Environment.Staging:
                        return "https://api-stag.veracity.com/veracity/datafabric/data/api/1/";
                    case Environment.Production:
                        return "https://api.veracity.com/veracity/datafabric/data/api/1/";
                    default:
                        throw new ArgumentOutOfRangeException($"Cannot accept {nameof(Env)} with value {Env}, allowed values are {Environment.Test}, {Environment.Staging} and {Environment.Production}");
                }
            }
        }

        public string SubscriptionKey { get; set; }
    }
}