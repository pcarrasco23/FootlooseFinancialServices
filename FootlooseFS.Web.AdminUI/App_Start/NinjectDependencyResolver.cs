using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FootlooseFS.Web.AdminUI
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IResolutionRoot _resolutionRoot;

        public NinjectDependencyResolver(IResolutionRoot kernel)
        {
            _resolutionRoot = kernel;
        }

        public object GetService(Type serviceType)
        {
            IRequest request = _resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return _resolutionRoot.Resolve(request).SingleOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            IRequest request = _resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return _resolutionRoot.Resolve(request);
        }
    }
}