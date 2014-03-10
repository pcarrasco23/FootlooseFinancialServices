using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Models
{
    public class Phone
    {
        public int PersonID { get; set; }
        public int PhoneTypeID { get; set; }
        public string Number { get; set; }

        public virtual PhoneType PhoneType { get; set; }
    }
}
