using Application.DTOs.Responses;
using Application.Services.Impls;
using Application.Services.Interfaces;
using Common.Pagination;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace exammaster_api.Controllers
{
    [Route("api/leaderboards")]
    [ApiController]
    public class LeaderBoardController : ControllerBase
    {
        private readonly ILeaderBoardService _leaderBoardService;
        public LeaderBoardController(ILeaderBoardService leaderBoardService, IUserService userService)
        {
            _leaderBoardService = leaderBoardService;
        }

        [HttpGet("top")]
        public async Task<ActionResult<ApiResponse<PageResult<LeaderboardUserResponse>>>> GetTopUserOrderByPoints([FromQuery] PageRequest pageRequest)
        {

            var result = await _leaderBoardService.GetTopUsersAsync(pageRequest);

            return Ok(new ApiResponse<PageResult<LeaderboardUserResponse>>
            {
                Message = $"Retrieved top {pageRequest.Size} user successfully",
                Result = result
            });
        }
    }
}
