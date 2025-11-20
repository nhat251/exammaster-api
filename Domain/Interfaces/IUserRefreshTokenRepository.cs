using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRefreshTokenRepository : IGenericRepository<UserRefreshToken, Guid>
    {
        public Task<UserRefreshToken?> GetByOldTokenStringAsync(string refreshTokenStr);
        public Task<UserRefreshToken?> GetByTokenStringAsync(string refreshTokenStr);
        public Task RemoveByUserIdAsync(string userId);
    }
}
