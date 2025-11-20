using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.AppDbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{

    public class UserRefreshTokenRepository : GenericRepository<UserRefreshToken, Guid>, IUserRefreshTokenRepository
    {

        public UserRefreshTokenRepository(ApplicationDbContext context) : base(context) { }

        public async Task<UserRefreshToken?> GetByTokenStringAsync(string refreshTokenStr)
        {
            return await _dbSet.FirstOrDefaultAsync(rt => rt.RefreshTokenString == refreshTokenStr);
        }

        public async Task<UserRefreshToken?> GetByOldTokenStringAsync(string refreshTokenStr)
        {
            return await _dbSet.FirstOrDefaultAsync(rt => rt.ReplacedByToken == refreshTokenStr);
        }

        public async Task RemoveByUserIdAsync(string userId)
        {
            await _dbSet.Where(rt => rt.UserId == userId).ExecuteDeleteAsync();
        }

    }
}
