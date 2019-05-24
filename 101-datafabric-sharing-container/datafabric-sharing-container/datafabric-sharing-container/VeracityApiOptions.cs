using System;

namespace datafabric_sharing_container
{
    public class VeracityApiOptions
    {
        public string AccessToken { get; set; }
        public string BaseUrl { get; set; }
        public string SubscriptionKey { get; set; }
    }


    public class DataFabricApiOptions : VeracityApiOptions { }
    public class MyServicesApiOptions : VeracityApiOptions { }
}