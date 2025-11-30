using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public class ExamAttempedResponse
    {
        public Guid Id { get; set; } = new Guid();
        // exam fields
        public Guid ExamId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Duration { get; set; }
        public int Points { get; set; }
        public int TotalQuestion { get; set; }
        public int RequiredPercentToPass { get; set; }
        public IEnumerable<TagResponse> Tags { get; set; } = Enumerable.Empty<TagResponse>();

        // attemp fields
        public Guid AttempId { get; set; }
        public int CompletedCount { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? ExpiredAt { get; set; }

        // favourite field
        public bool IsFavourite { get; set; } = false;
    }
}
