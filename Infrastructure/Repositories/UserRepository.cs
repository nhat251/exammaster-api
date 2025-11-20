using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.AppDbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser, string>, IUserRepository
    {
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;


        public UserRepository(ApplicationDbContext context, IPasswordHasher<ApplicationUser> passwordHasher) : base(context)
        {
            _passwordHasher = passwordHasher;
        }

        public async Task<ApplicationUser?> FindByUserNameAsync(string username)
        => await _dbSet.FirstOrDefaultAsync(u => u.UserName == username);

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
            => await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

        public bool CheckPassword(ApplicationUser user, string password)
            => _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password) == PasswordVerificationResult.Success;

        public async Task CreateAsync(ApplicationUser user, string password)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            _dbSet.Add(user);
            await _context.SaveChangesAsync();
        }

        

    }
}
