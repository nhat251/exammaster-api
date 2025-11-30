using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public class ExamResponse
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Duration { get; set; }
        public int Points { get; set; }
        public int TotalQuestion { get; set; }
        public IEnumerable<TagResponse> Tags { get; set; } = Enumerable.Empty<TagResponse>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int PassPercent { get; set; }

    }
}
