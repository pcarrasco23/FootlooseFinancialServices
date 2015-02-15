using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootlooseFS.Web.Service.Models
{
    public class AccountViewModel
    {
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public decimal AccountBalance { get; set; }
        public string AccountType { get; set; }
    }
}