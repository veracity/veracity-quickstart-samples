using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(VeracityWebApp.Startup))]

namespace VeracityWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
