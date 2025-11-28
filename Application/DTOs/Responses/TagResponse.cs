using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public class TagResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
