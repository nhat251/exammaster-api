using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IExamService
    {
        public Task<PageResult<ExamAttempedResponse>> GetUnFinishedExams(string userId, PageRequest pageRequest);
        public Task<string> MarkAsFavourited(Guid examId, string userId);
        public Task<string> UnMarkAsFavourited(Guid examId, string userId);
    }
}
