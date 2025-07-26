using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services.Interface
{
    public interface IUserService
    {
        public Task AddUserAsync(User user);
        public Task DeleteUserAsync(Guid userId);
        public Task<List<User>> GetAllUsersAsync();
        public Task<User?> GetUserByUsernameAsync(string userName);
        public User? GetByUsername(string username);
        public Task<User?> GetUserByIdAsync(Guid userId);
        public Task UpdateUserAsync(User user);
    }
}
