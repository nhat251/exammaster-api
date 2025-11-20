using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TokenInvalidRepository : GenericRepository<InvalidToken, Guid>, ITokenInvalidRepository
    {
        public TokenInvalidRepository(ApplicationDbContext context) : base(context) { }
        public async Task<bool> IsTokenInvalidAsync(string token)
        {
            var invalidToken = await GetAsync(invalid => invalid.Token == token);
            return invalidToken != null;
        }

    }
}
