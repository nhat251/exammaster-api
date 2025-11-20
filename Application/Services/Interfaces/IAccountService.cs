using Application.DTOs.Requests;
using Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<AuthResponse> LoginAsync(LoginRequest loginRequest);
        public Task<AuthResponse> RegisterAsync(RegisterRequest registerRequest);
        public Task<TokenResponse> RefreshTokenAsync(string refreshTokenStr);
        public Task<bool> LogOutAsync(string accessToken);
    }
}
