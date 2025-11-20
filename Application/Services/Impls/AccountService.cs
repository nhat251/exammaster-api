using Application.DTOs.Responses;
using Application.DTOs.Requests;
using Application.Services.Interfaces;
using Config;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Common.Exceptions;

namespace Application.Services.Impls
{
    public class AccountService : IAccountService
    {

        private readonly ITokenService _tokenService;

        private readonly JwtUtils _jwtUtils;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AccountService> _logger;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public AccountService(ITokenService tokenService, JwtUtils jwtUtils, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtSettings jwtSettings, ILogger<AccountService> logger)
        {
            _tokenService = tokenService;
            _jwtUtils = jwtUtils;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings;
            _logger = logger;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest loginRequest)
        {
            // Log user attempt
            _logger.LogInformation("Login attempt for username: {UserName}", loginRequest.UserName);

            var user = await _userManager.FindByNameAsync(loginRequest.UserName);

            // N?u user không t?n t?i, v?n check fake password d? tránh timing attack
            if (user == null)
            {
                _logger.LogWarning("Invalid login attempt - user not found: {UserName}", loginRequest.UserName);

                // fake check passwork hash
                //            _userManager.PasswordHasher.VerifyHashedPassword(new ApplicationUser(), new PasswordHasher<ApplicationUser>()
                //.HashPassword(new ApplicationUser(), "fake_password"), "fake");

                await Task.Delay(200);

                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Invalid username or password"
                };
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, isPersistent: false, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                _logger.LogWarning("Invalid password attempt for user: {UserName}", loginRequest.UserName);
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Invalid username or password"
                };
            }

            return await CreateSuccessfulAuthResponse(user);
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var existingUser = await _userManager.FindByNameAsync(registerRequest.UserName);
            if (existingUser != null)
            {
                throw new AppException(ErrorCode.USER_EXISTED);
            }

            var newUser = new ApplicationUser
            {
                UserName = registerRequest.UserName,
                FullName = registerRequest.FullName,
                Email = registerRequest.Email
            };

            var createResult = await _userManager.CreateAsync(newUser, registerRequest.Password);

            if (!createResult.Succeeded)
            {
                var errorMessage = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new AppException(ErrorCode.REGISTER_FAILED, $"{errorMessage}");
            }

            await _userManager.AddToRoleAsync(newUser, "User");

            // sign in sau khi dang ky
            await _signInManager.SignInAsync(newUser, isPersistent: false);

            var response = await CreateSuccessfulAuthResponse(newUser);

            scope.Complete();

            return response;
        }



        public async Task<TokenResponse> RefreshTokenAsync(string refreshTokenStr)
        {
            _logger.LogWarning("Refresh token requested: {RefreshToken}", refreshTokenStr);
            if (refreshTokenStr == null)
            {
                _logger.LogWarning("Refresh token is null");
                throw new AppException(ErrorCode.INVALID_REFRESH_TOKEN);
            }

            UserRefreshToken? userRefreshToken = await _tokenService.GetRefreshTokenAsync(p => p.RefreshTokenString == refreshTokenStr);

            if (userRefreshToken == null)
            {
                UserRefreshToken? oldRefreshToken = await _tokenService.GetRefreshTokenByOldTokenStringAsync(refreshTokenStr);

                if (oldRefreshToken != null)
                {
                    _logger.LogWarning("Refresh token is old => CO NGUOI VAO NICK NE {UserName}", oldRefreshToken.User.UserName);
                }
                throw new AppException(ErrorCode.INVALID_REFRESH_TOKEN);
            }

            if (userRefreshToken.IsExpired())
            {
                _logger.LogWarning("Refresh token is expired for user: {UserName}", userRefreshToken.User.UserName);
                throw new AppException(ErrorCode.INVALID_REFRESH_TOKEN);
            }

            var roles = await _userManager.GetRolesAsync(userRefreshToken.User);

            var newAccessToken = _jwtUtils.GenerateAccessToken(userRefreshToken.User.Id, userRefreshToken.User.UserName, roles);

            userRefreshToken = await UpdateUserRefreshTokenAsync(userRefreshToken);

            return new TokenResponse
            {
                AccessToken = newAccessToken,
                UserRefreshToken = userRefreshToken
            };
        }

        public async Task<bool> LogOutAsync(string accessToken)
        {

            await _tokenService.InvalidAccessToken(new InvalidToken
            {
                Token = accessToken,
                ExpiredAt = _jwtUtils.GetAccessTokenExpiredTime(accessToken)
            });

            await _tokenService.RemoveAllRefreshTokenOfUserAsync(accessToken);

            return await Task.FromResult(true);
        }

        private async Task<AuthResponse> CreateSuccessfulAuthResponse(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            // remove neu co refresh token cu
            await _tokenService.RemoveRefreshTokenByUserIdAsync(user.Id);

            var userRefreshToken = await AddUserRefreshTokenToDatabaseAsync(user);

            return new AuthResponse
            {
                IsSuccess = true,
                Message = "Login Successfully",
                AccessToken = _jwtUtils.GenerateAccessToken(user.Id, user.UserName!, userRoles),
                UserRefreshToken = userRefreshToken
            };
        }

        private async Task<UserRefreshToken> UpdateUserRefreshTokenAsync(UserRefreshToken userRefreshToken)
        {
            userRefreshToken.ReplacedByToken = userRefreshToken.RefreshTokenString;
            userRefreshToken.RefreshTokenString = _jwtUtils.GenerateRefreshToken();
            userRefreshToken.CreatedAt = DateTime.UtcNow;
            userRefreshToken.ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDays);

            return await _tokenService.UpdateRefreshTokenAsync(userRefreshToken);
        }

        private async Task<UserRefreshToken> AddUserRefreshTokenToDatabaseAsync(ApplicationUser user)
        {
            string refreshToken = _jwtUtils.GenerateRefreshToken();
            return await _tokenService.AddRefreshTokenAsync(new UserRefreshToken
            {
                RefreshTokenString = refreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDays),
                UserId = user.Id,
                User = user
            });
        }

    }
}
