using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Models
{
    public class AccountTransaction
    {
        public int AccountTransactionID { get; set; }
        public int AccountID { get; set; }
        public int TransactionTypeID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }

        public virtual TransactionType TransactionType { get; set; }
    }
}
