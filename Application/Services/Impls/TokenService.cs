using Application.Services.Interfaces;
using Config;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Impls
{
    public class TokenService : ITokenService
    {
        private readonly ITokenInvalidRepository _tokenInvalidRepository;
        private readonly IUserRefreshTokenRepository _refreshTokenRepository;

        private readonly JwtUtils _jwtUtils;

        public TokenService(ITokenInvalidRepository tokenInvalidRepository, IUserRefreshTokenRepository refreshTokenRepository, JwtUtils jwtUtils)
        {
            _tokenInvalidRepository = tokenInvalidRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtUtils = jwtUtils;
        }

        // Refresh Token
        public async Task<UserRefreshToken?> GetRefreshTokenAsync(Expression<Func<UserRefreshToken, bool>> predicate)
        {
            return await _refreshTokenRepository.GetAsync(predicate: (predicate), includes: rt => rt.User);
        }
        public async Task<UserRefreshToken?> GetRefreshTokenByOldTokenStringAsync(string refreshTokenStr)
        {
            return await _refreshTokenRepository.GetByOldTokenStringAsync(refreshTokenStr);
        }
        public async Task RemoveRefreshTokenByUserIdAsync(string id)
        {
            await _refreshTokenRepository.RemoveByUserIdAsync(id);
        }
        public async Task<UserRefreshToken> UpdateRefreshTokenAsync(UserRefreshToken userRefreshToken)
        {
            return await _refreshTokenRepository.UpdateAsync(userRefreshToken);
        }
        public async Task<UserRefreshToken> AddRefreshTokenAsync(UserRefreshToken userRefreshToken)
        {
            return await _refreshTokenRepository.AddAsync(userRefreshToken);
        }

        // Access Token
        public async Task InvalidAccessToken(InvalidToken invalidToken)
        {
            await _tokenInvalidRepository.AddAsync(invalidToken);
        }
        public async Task<bool> IsTokenInvalidAsync(string jwtToken)
        {
            return await _tokenInvalidRepository.IsTokenInvalidAsync(jwtToken);
        }
        public async Task RemoveAllRefreshTokenOfUserAsync(string accessToken)
        {
            string userId = _jwtUtils.GetClaim(accessToken, JwtRegisteredClaimNames.Sub)!;

            await _refreshTokenRepository.RemoveByUserIdAsync(userId);
        }

    }
}
