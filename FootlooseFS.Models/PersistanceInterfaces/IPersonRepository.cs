using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Models
{
    public interface IPersonRepository : IRepository<Person>
    {
        Person Find(int personId, PersonIncludes personIncludes);
    }
}
