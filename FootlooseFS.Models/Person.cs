using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Models
{
    public class Person
    {
        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public virtual ICollection<PersonAccount> Accounts { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
        public virtual ICollection<PersonAddressAssn> Addresses { get; set; }
        public virtual PersonLogin Login { get; set; }
    }
}
