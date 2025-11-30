using Application.DTOs.Responses;
using Common.Pagination;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ILeaderBoardService
    {
        public Task<PageResult<LeaderboardUserResponse>> GetTopUsersAsync(PageRequest pageRequest);
    }
}
