using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebSecurityOverview.Startup))]
namespace WebSecurityOverview
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
