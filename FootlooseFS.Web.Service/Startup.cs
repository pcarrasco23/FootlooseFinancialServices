using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(FootlooseFS.Web.Service.Startup))]

namespace FootlooseFS.Web.Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            ConfigureAuth(app);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
        }
    }
}
