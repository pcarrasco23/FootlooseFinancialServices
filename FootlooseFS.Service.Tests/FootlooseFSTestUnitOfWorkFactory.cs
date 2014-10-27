using FootlooseFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Service.Tests
{
    public class FootlooseFSTestUnitOfWorkFactory : IFootlooseFSUnitOfWorkFactory
    {
        public IFootlooseFSUnitOfWork CreateUnitOfWork()
        {
            return new FootlooseFSTestUnitOfWork();
        }
    }
}
