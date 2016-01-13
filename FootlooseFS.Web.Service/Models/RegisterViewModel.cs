using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootlooseFS.Web.Service.Models
{
    public class RegisterViewModel
    {
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}