using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ITokenService
    {
        // Refresh Token
        Task<UserRefreshToken?> GetRefreshTokenAsync(Expression<Func<UserRefreshToken, bool>> predicate);
        Task<UserRefreshToken?> GetRefreshTokenByOldTokenStringAsync(string refreshTokenStr);
        Task RemoveRefreshTokenByUserIdAsync(string id);
        Task<UserRefreshToken> UpdateRefreshTokenAsync(UserRefreshToken userRefreshToken);
        Task<UserRefreshToken> AddRefreshTokenAsync(UserRefreshToken userRefreshToken);

        // Access Token
        Task InvalidAccessToken(InvalidToken invalidToken);
        Task<bool> IsTokenInvalidAsync(string jwtToken);
        Task RemoveAllRefreshTokenOfUserAsync(string accessToken);
    }
}
