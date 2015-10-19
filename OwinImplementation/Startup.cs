using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OwinImplementation.Startup))]
namespace OwinImplementation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
