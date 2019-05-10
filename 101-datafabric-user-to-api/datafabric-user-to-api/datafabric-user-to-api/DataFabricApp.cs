using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace datafabric_user_to_api
{
    /// <summary>
    /// Simple service that acquires a token from data fabric, calls to check our applications profile and prints the list of containers shared with this user, if any
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
            var profile = await _dataFabricClient.MyProfile();
            Console.WriteLine($"My User Profile is: ");
            PrintProperties(profile);

            Console.WriteLine($"\n========================");
            Console.WriteLine($"Containers shared with my user:");
            var containers = await _dataFabricClient.GetContainersSharedWithMe();
            containers.ForEach(c =>
            {
                Console.WriteLine();
                PrintProperties(c);
            });
        }


        private void PrintProperties<T>(T obj)
        {
            obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty).ToList().ForEach(p =>
            {
                Console.WriteLine($"\t {p.Name} : {p.GetValue(obj)}");
            });
        }
    }
}
