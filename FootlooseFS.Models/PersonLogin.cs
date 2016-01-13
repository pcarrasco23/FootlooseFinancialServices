using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Models
{
    [KnownType(typeof(Person))]
    public class PersonLogin
    {
        public int PersonID { get; set; }
        public string LoginID { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }

        public virtual Person Person { get; set; }
    }
}
