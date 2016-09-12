using System.Data.Entity;
using System.Web;
using Heroic.Web.IoC;
using System.Web.Http;
using System.Web.Mvc;
using HSKR.Identity;
using CTDT.Model;
//using CTDT.WebApi.Areas.HelpPage.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using StructureMap.Graph;


[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(CTDT.WebApi.StructureMapConfig), "Configure")]
namespace CTDT.WebApi
{
	public static class StructureMapConfig
	{
		public static void Configure()
		{
			IoC.Container.Configure(cfg =>
			{
				cfg.Scan(scan =>
				{
					scan.TheCallingAssembly();
					scan.WithDefaultConventions();
				});

				cfg.AddRegistry(new ControllerRegistry());
				cfg.AddRegistry(new MvcRegistry());
				cfg.AddRegistry(new ActionFilterRegistry(namespacePrefix: "CTDT.WebApi"));

				//Are you using ASP.NET Identity?  If so, you'll probably need to configure some additional services:
				
				//1) Make IUserStore injectable.  Replace 'ApplicationUser' with whatever your Identity user type is.
				//For<IUserStore<ApplicationUser>>().Use<UserStore<ApplicationUser>>();
				
				//2) Change AppDbContext to your application's Entity Framework context.
                //cfg.For<DbContext>().Use<DBContext>();
				
				//3) This will allow you to inject the IAuthenticationManager.  You may not need this, but you will if you 
				//   used the default ASP.NET MVC project template as a starting point!
				//For<IAuthenticationManager>().Use(ctx => ctx.GetInstance<HttpRequestBase>().GetOwinContext().Authentication);

				//TODO: Add other registries and configure your container (if needed)

                cfg.For<IUserStore<MyUser, long>>().Use(ctx => new UserStore<MyUser, MyRole, long, MyLogin, MyUserRole, MyClaim>(new MyDbContext()));

                cfg.For<IRoleStore<MyRole, long>>().Use(ctx => new RoleStore<MyRole, long, MyUserRole>(new MyDbContext()));

                cfg.For<IContext>().Use<DBContext>();

                //cfg.For<HelpController>().Use(ctx => new HelpController());

                cfg.For<IAuthenticationManager>().Use(ctx => ctx.GetInstance<HttpRequestBase>().GetOwinContext().Authentication);
			});

			var resolver = new StructureMapDependencyResolver();
			DependencyResolver.SetResolver(resolver);
			GlobalConfiguration.Configuration.DependencyResolver = resolver;
		}
	}
}