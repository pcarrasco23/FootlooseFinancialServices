using FootlooseFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.EnterpriseService
{
    public class PageOfPersonDocuments
    {
        public List<PersonDocument> Data { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
        public int TotalPageCount { get; private set; }
    }
}
