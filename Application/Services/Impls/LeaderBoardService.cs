using Application.DTOs.Responses;
using Application.Services.Interfaces;
using AutoMapper;
using Common.Pagination;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Impls
{
    public class LeaderBoardService : ILeaderBoardService
    {

        private readonly ILogger<LeaderBoardService> _logger;
        private readonly IMapper _mapper;
        private readonly IExamRepository _examRepository;
        private readonly IAttempExamRepository _attempExamRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICollectionRepository _collectionRepository;

        private readonly IDbProcedureExecutor _dbProcedureExecutor;




        public LeaderBoardService(IMapper mapper, ILogger<LeaderBoardService> logger, IExamRepository examRepository, IAttempExamRepository attempExamRepository, IUserRepository userRepository, ICollectionRepository collectionRepository, IDbProcedureExecutor dbProcedureExecutor)
        {
            _logger = logger;
            _mapper = mapper;
            _examRepository = examRepository;
            _attempExamRepository = attempExamRepository;
            _userRepository = userRepository;
            _collectionRepository = collectionRepository;
            _dbProcedureExecutor = dbProcedureExecutor;
        }

        public async Task<PageResult<LeaderboardUserResponse>> GetTopUsersAsync(PageRequest pageRequest)
        {
            var allUsers = await _dbProcedureExecutor.GetAllUsersAsync();

            int totalItems = allUsers.Count(); 

            int skip = (pageRequest.Page - 1) * pageRequest.Size;

            var pagedItems = allUsers
                .OrderByDescending(u => (int)u.TotalPoints)
                .ThenBy(u => (string)u.UserName)
                .Skip(skip)
                .Take(pageRequest.Size)
                .Select(u => new LeaderboardUserResponse
                {
                    UserId = (string)u.UserId,
                    UserName = (string)u.UserName,
                    FullName = (string)u.FullName,
                    TotalPoints = (int)u.TotalPoints
                })
                .ToList();

            return new PageResult<LeaderboardUserResponse>
            {
                Items = pagedItems,
                Page = pageRequest.Page,
                Size = pageRequest.Size,
                TotalItems = totalItems
            };
        }


    }
}
