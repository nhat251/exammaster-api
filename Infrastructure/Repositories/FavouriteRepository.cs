using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FavouriteRepository : GenericRepository<UserFavourite, Guid>, IFavouriteRepository
    {
        public FavouriteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> IsFavouriteExists(string userId, Guid examId)
        {
            return await _dbSet.AnyAsync(f => f.UserId == userId && f.ExamId == examId);
        }

    }
}
