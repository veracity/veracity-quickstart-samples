using System;
using System.Threading.Tasks;

namespace datafabric_app_to_app
{
    /// <summary>
    /// Simple service that acquires a token from data fabric, calls to check our applications profile and prints the list of containers shared with this application, if any
    /// </summary>
    public class DataFabricApp
    {
        private readonly DataClient _dataFabricClient;

        public DataFabricApp(DataClient client)
        {
            _dataFabricClient = client;
        }

        public async Task Run()
        {
            Console.WriteLine($"========================");
            Console.WriteLine($"Acquiring token..");
            var token = await _dataFabricClient.AcquireToken();
            Console.WriteLine($"Acquired token: {token.AccessToken}");

            Console.WriteLine($"\n========================");
            var appProfile = await _dataFabricClient.MyApplicationId();
            Console.WriteLine($"My application Id is: {appProfile.id}");

            Console.WriteLine($"\n========================");
            Console.WriteLine($"Containers shared with my app:");
            var containers = await _dataFabricClient.GetContainersSharedWithApp();
            containers.ForEach(c =>
            {
                Console.WriteLine($"\t container: {c.reference}");
            });
        }
    }
}
