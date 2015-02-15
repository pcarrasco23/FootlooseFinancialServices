using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Models
{
    public class Address
    {
        public int AddressID { get; set; }

        [Required]
        [StringLength(100)]
        public string StreetAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(5)]
        public string State { get; set; }

        [Required]
        [StringLength(20)]
        public string Zip { get; set; }
    }
}
