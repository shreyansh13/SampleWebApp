using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RISWebApp.Startup))]
namespace RISWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
