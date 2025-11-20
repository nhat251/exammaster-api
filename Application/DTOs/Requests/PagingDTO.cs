using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Requests
{
    public abstract class PagingDTO
    {
        public int currentPage { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string? SortBy { get; set; } = "Id";
        public string SortOrder { get; set; } = "asc";
    }
}
