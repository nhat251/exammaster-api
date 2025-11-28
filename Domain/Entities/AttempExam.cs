using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AttempExam : IEntity<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public required Guid ExamId { get; set; }
        public Exam Exam { get; set; } = null!;
        public int CompletedCount { get; set; } = 0;
        public bool IsFinished { get; set; } = false;
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiredAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? SubmitAt { get; set; }
    }
}
