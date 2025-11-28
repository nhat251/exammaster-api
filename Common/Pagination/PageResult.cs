using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Pagination
{
    public class PageResult<T> where T : class
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / Size);
    }
}
