using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootlooseFS.Web.Service.Models
{
    public class HolderAccountsViewModel
    {
        public IEnumerable<AccountViewModel> Accounts { get; set; }
        public decimal Total { get; set; }
        public IEnumerable<TransactionViewModel> Transactions { get; set; }
    }
}