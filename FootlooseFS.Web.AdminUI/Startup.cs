using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FootlooseFS.Web.AdminUI.Startup))]
namespace FootlooseFS.Web.AdminUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
