using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootlooseFS.Web.Service.Models
{
    public class TransactionViewModel
    {
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}