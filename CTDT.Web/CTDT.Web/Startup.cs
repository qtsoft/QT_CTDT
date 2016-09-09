using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CTDT.Web.Startup))]
namespace CTDT.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
