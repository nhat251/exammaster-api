using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Pagination
{
    public class PageRequest
    {
        public PageRequest() { }
        public PageRequest(int currentPage, int size, string? orderBy, string sortOrder)
        {
            Page = currentPage;
            Size = size;
            OrderBy = orderBy;
            SortOrder = sortOrder;
        }

        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string? OrderBy { get; set; } = "Id";
        public string SortOrder { get; set; } = "asc";
    }
}

