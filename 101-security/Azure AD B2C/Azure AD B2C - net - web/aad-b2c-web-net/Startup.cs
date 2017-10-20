using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(aad_b2c_web_net.Startup))]

namespace aad_b2c_web_net
{
    /// <summary>
    /// Startup class used by OWIN when starting app.
    /// Based on Microsoft tutorial:
    /// https://docs.microsoft.com/en-us/azure/active-directory-b2c/active-directory-b2c-devquickstarts-web-dotnet-susi
    /// </summary>
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}