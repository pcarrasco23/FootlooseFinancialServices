using FootlooseFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootlooseFS.Web.AdminUI.Models
{
    public class SavePersonResult
    {
        public String Message { get; set; }
        public Person Person { get; set; }
        public int PersonID { get; set; }
        public int HomeAddressID { get; set; }
        public int WorkAddressID { get; set; }
        public int AlternateAddressID { get; set; }
    }
}