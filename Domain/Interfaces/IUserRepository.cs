using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<ApplicationUser, string>
    {
        public Task<ApplicationUser?> FindByUserNameAsync(string username);
        public Task<ApplicationUser?> FindByEmailAsync(string email);
        public bool CheckPassword(ApplicationUser user, string password);
        public Task CreateAsync(ApplicationUser user, string password);

    }
}
