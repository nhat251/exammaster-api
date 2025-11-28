using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ExamRepository : GenericRepository<Exam, Guid>, IExamRepository
    {
        public ExamRepository(ApplicationDbContext context) : base(context) { }

    }
}
