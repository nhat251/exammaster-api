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
    public interface IUserService
    {
        Task<UserResponse> GetByIdAsync(string id);

        Task<PageResult<UserResponse>> GetPagedUsers(GetUserRequest request);

    }
}
