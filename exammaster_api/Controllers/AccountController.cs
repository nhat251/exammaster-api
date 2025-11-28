using System.Runtime.CompilerServices;
using Application;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Services.Interfaces;
using Azure.Core;
using Config;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;



namespace exammaster_api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ICookieService _cookieService;

        public AccountController(IAccountService accountService, ICookieService cookieService)
        {
            _accountService = accountService;
            _cookieService = cookieService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<Object>>> Login([FromBody] LoginRequestDTO loginRequest)
        {
            AuthResponse res = await _accountService.LoginAsync(loginRequest);

            if (res.IsSuccess)
            {
                _cookieService.SetCookie(Response, "refreshToken", res.UserRefreshToken.RefreshTokenString, res.UserRefreshToken.ExpiresAt);
            }
            else return Ok(new ApiResponse<bool>
            {
                Result = false,
                Message = res.Message
            });

            return Ok(new ApiResponse<Object>
            {
                Result = new { AccessToken = res.AccessToken },
                Message = res.Message
            });

        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<Object>>> Register([FromBody] RegisterRequestDTO registerRequest)
        {
            AuthResponse res = await _accountService.RegisterAsync(registerRequest);

            _cookieService.SetCookie(Response, "refreshToken", res.UserRefreshToken.RefreshTokenString, res.UserRefreshToken.ExpiresAt);

            return Ok(new ApiResponse<Object>
            {
                Result = new { AccessToken = res.AccessToken },
                Message = "Register successfully"
            });
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<Object>>> RefreshToken()
        {
            var refreshTokenStr = Request.Cookies["refreshToken"];

            TokenResponse res = await _accountService.RefreshTokenAsync(refreshTokenStr);
            _cookieService.SetCookie(Response, "refreshToken", res.UserRefreshToken.RefreshTokenString, res.UserRefreshToken.ExpiresAt);

            return Ok(new ApiResponse<Object>
            {
                Result = new { AccessToken = res.AccessToken },
                Message = "Refresh token successfully"
            });

        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<bool>>> LogOut()
        {
            var accessToken = JwtUtils.ParseAccessTokenFromHeader(Request.Headers.Authorization.ToString());

            bool res = await _accountService.LogOutAsync(accessToken);

            _cookieService.SetCookie(Response, "refreshToken", "", DateTime.UtcNow.AddMinutes(-1));

            return Ok(new ApiResponse<bool>
            {
                Result = true,
                Message = "Logout successfully"
            });
        }
    }
}
