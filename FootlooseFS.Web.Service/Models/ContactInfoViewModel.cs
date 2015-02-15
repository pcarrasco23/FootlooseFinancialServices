using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootlooseFS.Web.Service.Models
{
    public class ContactInfoViewModel
    {        
        public string EmailAddress { get; set; }
        public List<AddressViewModel> Addresses { get; set; }
        public List<PhoneViewModel> PhoneNumbers { get; set; }
    }
}