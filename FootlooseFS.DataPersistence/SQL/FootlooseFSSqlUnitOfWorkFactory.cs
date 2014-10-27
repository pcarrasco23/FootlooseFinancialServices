using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FootlooseFS.Models;

namespace FootlooseFS.DataPersistence
{
    public class FootlooseFSSqlUnitOfWorkFactory : IFootlooseFSUnitOfWorkFactory
    {
        public IFootlooseFSUnitOfWork CreateUnitOfWork()
        {
            return new FootlooseFSSqlUnitOfWork();
        }
    }
}
