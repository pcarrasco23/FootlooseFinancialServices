using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Models
{
    public class Phone
    {
        public int PersonID { get; set; }
        public int PhoneTypeID { get; set; }

        [Required]
        [StringLength(100)]
        public string Number { get; set; }

        public virtual PhoneType PhoneType { get; set; }
    }
}
