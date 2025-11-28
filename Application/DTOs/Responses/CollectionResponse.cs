using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public class CollectionResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int TotalExams { get; set; }
        public int TotalDurations { get; set; }

        public ICollection<ExamResponse> Exams { get; set; } = new List<ExamResponse>();

        public ICollection<TagResponse> Tags { get; set; } = new List<TagResponse>();
    }
}
