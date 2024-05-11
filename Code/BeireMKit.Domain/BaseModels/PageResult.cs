using System;
using System.Collections.Generic;
using System.Text;

namespace BeireMKit.Domain.BaseModels
{
    public class PageResult
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)Limit);
        public int PreviousPage => Offset > 1 ? Offset - 1 : Offset;
        public int NextPage => Offset < TotalPages ? Offset + 1 : Offset;

        public PageResult() { }

        public PageResult(int limit, int offset, int totalCount)
        {
            Limit = limit;
            Offset = offset;
            TotalCount = totalCount;
        }
    }
}
