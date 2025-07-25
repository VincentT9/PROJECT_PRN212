using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        UserDAO userDAO = new UserDAO();

        public Task AddUserAsync(User user)
        {
            return userDAO.AddUserAsync(user);
        }

        public Task DeleteUserAsync(Guid userId)
        {
            return userDAO.DeleteUserAsync(userId);
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            return userDAO.GetAllUsersAsync();
        }

        public Task<User?> GetUserByIdAsync(Guid userId)
        {
            return userDAO.GetUserByIdAsync(userId);
        }

        public Task<User?> GetUserByUsernameAsync(string userName)
        {
            return userDAO.GetUserByUsernameAsync(userName);
        }

        public Task UpdateUserAsync(User user)
        {
            return userDAO.UpdateUserAsync(user);
        }

        public Task<List<User>> GetUsersByRoleAsync(int role)
        {
            return userDAO.GetUsersByRoleAsync(role);
        }
    }
}
