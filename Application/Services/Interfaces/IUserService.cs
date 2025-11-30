using Application.DTOs.Requests;
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
    public interface IUserService
    {
        public Task<UserResponse> GetByIdAsync(string id);
        public Task<PageResult<UserResponse>> GetPagedUsers(PageRequest request);
        public Task<UserResponse> MapUserResponseAsync(ApplicationUser user);
    }
}
