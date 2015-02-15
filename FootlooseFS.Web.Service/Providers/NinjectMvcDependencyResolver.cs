using Ninject;
using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Web.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootlooseFS.Web.Service
{
    public class NinjectMvcDependencyResolver : NinjectDependencyScope, System.Web.Mvc.IDependencyResolver
    {
        private IKernel kernel;

        public NinjectMvcDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }

        public object GetService(Type serviceType)
        {
            IRequest request = kernel.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return kernel.Resolve(request).SingleOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            IRequest request = kernel.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return kernel.Resolve(request);
        }
    }
}