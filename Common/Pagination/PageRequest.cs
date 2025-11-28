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
        public PageRequest(int currentPage, int size, string? sortBy, string sortOrder)
        {
            Page = currentPage;
            Size = size;
            SortBy = sortBy;
            SortOrder = sortOrder;
        }

        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string? SortBy { get; set; } = "Id";
        public string SortOrder { get; set; } = "asc";
    }
}
