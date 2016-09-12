using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SIGN.WEB.Startup))]
namespace SIGN.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
