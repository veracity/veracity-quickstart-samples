using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datafabric_sharing_container
{
    class Program
    {
        static void Main(string[] args)
        {
            //You can just load the data client with the required options directly, but setting this console up with Dependency Injection, the transition to using this in .NET Core Web Application will be minimal
            //Create services
            var services = new ServiceCollection();
            ConfigureServices(services);

            //Create service provider
            var serviceProvider = services.BuildServiceProvider();

            //Run application
            serviceProvider.GetService<DataFabricApp>().Run().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"configuration.json", false, true)
                .Build();
                
            services.AddOptions();
            services.Configure<DataFabricApiOptions>(o => { configuration.GetSection("VeracityDataApiOptions").Bind(o); });
            services.Configure<MyServicesApiOptions>(o => { configuration.GetSection("VeracityServiceAPIOptions").Bind(o); });
            services.AddSingleton<IConfiguration>(configuration);
            services.AddTransient<DataClient>();
            services.AddTransient<DataFabricApp>();
        }
    }
}
