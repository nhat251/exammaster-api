using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Services.Interfaces;
using Application.Specifications;
using AutoMapper;
using Common.Exceptions;
using Common.Pagination;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Impls
{
    public class UserService(IUserRepository _userRepository, IAttempExamRepository _attempExamRepository, IMapper _mapper, ILogger<UserService> _logger) : IUserService
    {
        public async Task<UserResponse> GetByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id) ?? throw new AppException(ErrorCode.USER_NOT_FOUND);
            return await MapUserResponseAsync(user);
        }

        public async Task<PageResult<UserResponse>> GetPagedUsers(PageRequest pageRequest)
        {
            //var users = await _userRepository.GetPagedAsync(predicate: UserSpecifications.GetUserByRequest(request), pageRequest: pageRequest);
            var users = await _userRepository.GetPagedAsync(pageRequest: pageRequest);
            return _mapper.Map<PageResult<UserResponse>>(users);
        }

        public async Task<UserResponse> MapUserResponseAsync(ApplicationUser user)
        {

            // lay ra nhung attempt exam da hoan thanh cua user, chua tinh pass va trung exam
            var attemps = await _attempExamRepository.GetAllAsync(
                predicate: ae => ae.UserId == user.Id && ae.IsFinished,
                includes: ae => ae.Exam);

            var totalPoints = attemps
                .Where(a => a.IsPassed())
                .GroupBy(a => a.ExamId)
                .Sum(g => g.First().Exam.Points);

            var response = _mapper.Map<UserResponse>(user);
            response.TotalPoints = totalPoints;

            return response;
        }

        
    }
}
