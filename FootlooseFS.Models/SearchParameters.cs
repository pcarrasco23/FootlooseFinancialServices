using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootlooseFS.Models
{
    public class SearchParameters
    {
        public int PageNumber { get; set; }
        public int NumberRecordsPerPage { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public Dictionary<string, string> SearchCriteria { get; set; }
    }
}