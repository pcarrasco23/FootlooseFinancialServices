using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Models
{
    [KnownType(typeof(Address))]
    [KnownType(typeof(AddressType))]
    public class PersonAddressAssn
    {
        public int PersonID { get; set; }
        public int AddressID { get; set; }
        public int AddressTypeID { get; set; }

        public virtual Address Address { get; set; }
        public virtual AddressType AddressType { get; set; }
    }
}
