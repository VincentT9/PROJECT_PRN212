using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories.Interface
{
    public interface IUserRepository
    {
        public Task AddUserAsync(User user);
        public Task DeleteUserAsync(Guid userId);
        public Task<List<User>> GetAllUsersAsync();
        public Task<User?> GetUserByUsernameAsync(string userName);
        public Task<User?> GetUserByIdAsync(Guid userId);
        public Task UpdateUserAsync(User user);
        public Task<List<User>> GetUsersByRoleAsync(int role);
    }
}
