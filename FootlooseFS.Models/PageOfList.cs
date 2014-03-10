using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Models
{
    public class PageOfList<T>
    {
        public PageOfList(IEnumerable<T> Data, int PageIndex, int PageSize, int TotalItemCount)
        {
            this.Data = new List<T>();
            this.Data.AddRange(Data);

            this.PageIndex = PageIndex;
            this.PageSize = PageSize;
            this.TotalItemCount = TotalItemCount;

            if (PageSize == -1)
                this.TotalPageCount = 1;
            else
                this.TotalPageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
        }

        public List<T> Data { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
        public int TotalPageCount { get; private set; }
        public Dictionary<string, string> SearchCriteria { get; set; }
    }
}
