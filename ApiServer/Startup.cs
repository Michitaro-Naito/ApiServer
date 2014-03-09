using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ApiServer.Startup))]
namespace ApiServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
