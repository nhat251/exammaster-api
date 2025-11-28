using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Tag : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Exam> Exams { get; set; } = new List<Exam>();
    }
}
