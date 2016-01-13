using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Models
{
    [KnownType(typeof(AccountTransaction))]
    [KnownType(typeof(AccountType))]
    public class Account
    {
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public decimal AccountBalance { get; set; }
        public int AccountTypeID { get; set; }

        public virtual AccountType AccountType { get; set; }
        public virtual ICollection<AccountTransaction> Transactions { get; set; }
    }
}
