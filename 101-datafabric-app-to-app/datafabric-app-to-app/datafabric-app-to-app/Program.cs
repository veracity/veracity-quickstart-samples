using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datafabric_app_to_app
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
            services.Configure<DataClientOptions>(o => { configuration.GetSection("DataClientOptions").Bind(o); });
            services.AddTransient<DataClient>();
            services.AddTransient<DataFabricApp>();
        }
    }
}
