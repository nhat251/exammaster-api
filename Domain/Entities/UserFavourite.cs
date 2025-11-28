using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserFavourite : IEntity<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public required  Guid ExamId { get; set; }
        public Exam Exam { get; set; } = null!;
    }
}
