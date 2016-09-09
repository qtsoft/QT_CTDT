using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CTDT.DN.Startup))]
namespace CTDT.DN
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
