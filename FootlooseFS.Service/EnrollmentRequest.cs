using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Service
{
    public class EnrollmentRequest
    {      
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
