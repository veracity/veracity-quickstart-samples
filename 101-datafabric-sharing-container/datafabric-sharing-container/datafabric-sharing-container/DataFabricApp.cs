using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace datafabric_sharing_container
{
    /// <summary>
    /// Simple service that acquires a token from data fabric, calls to check our applications profile and prints the list of containers shared with this user, if any
    /// </summary>
    public class DataFabricApp
    {
        private readonly DataClient _dataFabricClient;
        private readonly IConfiguration _config;


        public DataFabricApp(DataClient client, IConfiguration config)
        {
            _dataFabricClient = client;
            _config = config;
        }

        public async Task Run()
        {
            
            // Load users from config
            var users = _config["UserIds"].Replace(" ", "").Split(",").Select(Guid.Parse);
            // Load my containers
            var containers = await _dataFabricClient.GetContainersSharedWithMe();
            Console.WriteLine($"Loaded my containers, I have {containers.Count} containers.");
            // Select my first container
            var container = containers.First();
            Console.WriteLine($"Selecte container to share: {container.metadata.title}");
            // Select a key template, where the key I will share has read/write/list access to my container for 8 hours
            var keyTemplate = await _dataFabricClient.GetKeyTemplate(readAccess: true, writeAccess: true, deleteAccess: false, listAccess: true, durationHours: 8);
            Console.WriteLine($"Selected keytemplate: {keyTemplate.name}");
            // Cannot renew key
            var autoRefreshable = false;
            // Comment received by users I share with when they acquire key
            var comment = "Please upload files to this container starting today!";

            Console.WriteLine($"========== Starting Sharing Container ======== ");
            foreach (var user in users)
            {
                Console.WriteLine($"\t Sharing container: {container.id} with {user}");
                var shareId = await _dataFabricClient.ShareContainerWithUser(container, user, keyTemplate, comment, autoRefreshable);
                Console.WriteLine($"\t Successfully shared! Sharing id: {shareId}" );
            }

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

