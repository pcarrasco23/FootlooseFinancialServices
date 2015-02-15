using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootlooseFS.Web.Service.Models
{
    public class AddressViewModel
    {
        public int AddressID { get; set; }
        public string Type { get; set; }
        public int AddressTypeID { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; } 
    }
}