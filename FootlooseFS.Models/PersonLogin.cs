using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [StringLength(50)]
        public string LoginID { get; set; }

        [Required]
        [StringLength(64)]
        public string HashedPassword { get; set; }

        [Required]
        [StringLength(64)]
        public string Salt { get; set; }

        public virtual Person Person { get; set; }
    }
}
