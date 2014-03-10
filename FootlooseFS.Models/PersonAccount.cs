using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Models
{
    public class PersonAccount
    {
        public int PersonID { get; set; }
        public int AccountID { get; set; }
        public int RelationshipTypeID { get; set; }

        public virtual Account Account { get; set; }
        public virtual RelationshipType RelationshipType { get; set; }
    }
}
