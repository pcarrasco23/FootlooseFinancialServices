using FootlooseFS.Service;
using FootlooseFS.Models;
using FootlooseFS.DataPersistence;
using Microsoft.Owin;
using Ninject;
using Owin;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(FootlooseFS.Web.AdminUI.Startup))]
namespace FootlooseFS.Web.AdminUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var kernel = new StandardKernel();

            kernel.Bind<IFootlooseFSService>().To<FootlooseFSService>();
            kernel.Bind<IFootlooseFSUnitOfWorkFactory>().To<FootlooseFSSqlUnitOfWorkFactory>();

            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}
