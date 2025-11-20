using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Services.Interfaces;
using Application.Specifications;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Common.Pagination;
using Common.Exceptions;

namespace Application.Services.Impls
{
    public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
    {
        public async Task<UserResponse> GetByIdAsync(string id)
        {
            var user = await userRepository.GetByIdAsync(id) ?? throw new AppException(ErrorCode.USER_NOT_FOUND);
            return mapper.Map<UserResponse>(user);
        }

        public async Task<PageResult<UserResponse>> GetPagedUsers(GetUserRequest request)
        {
            var pageRequest = new PageRequest(request.currentPage, request.Size, request.SortBy, request.SortOrder);
            var users = await userRepository.GetPagedAsync(predicate: UserSpecifications.GetUserByRequest(request), pageRequest: pageRequest);
            return mapper.Map<PageResult<UserResponse>>(users);
        }
    }
}
