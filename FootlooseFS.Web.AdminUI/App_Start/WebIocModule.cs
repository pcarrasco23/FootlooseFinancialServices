using FootlooseFS.Service;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootlooseFS.Web.AdminUI
{
    public class WebIocModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IFootlooseFSService>().To<FootlooseFSService>();
        }
    }
}