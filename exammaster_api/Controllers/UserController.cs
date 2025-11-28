using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Services.Interfaces;
using Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exammaster_api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(IUserService _userService) : ControllerBase
    {
        [HttpGet("my-info")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserResponse>>> GetMyInfo()
        {
            //var user = _userManager.getUserAsync(User);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userService.GetByIdAsync(userId!);


            return Ok(new ApiResponse<UserResponse>
            {
                Message = "User info retrieved successfully",
                Result = user
            });
        }

        [HttpPost("paging")]
        [Authorize]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<PageResult<UserResponse>>>> GetUsersWithPagination([FromBody] GetUserRequest getUserRequest)
        {
            var result = await _userService.GetPagedUsers(getUserRequest);

            return Ok(new ApiResponse<PageResult<UserResponse>>
            {
                Message = "User info retrieved successfully",
                Result = result
            });
        }
    }

}
