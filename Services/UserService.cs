using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;
using Repositories.Interface;
using Services.Interface;

namespace Services
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public Task AddUserAsync(User user)
        {
            return _userRepository.AddUserAsync(user);
        }

        public Task DeleteUserAsync(Guid userId)
        {
            return _userRepository.DeleteUserAsync(userId);
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            return _userRepository.GetAllUsersAsync();
        }

        public Task<User?> GetUserByIdAsync(Guid userId)
        {
            return _userRepository.GetUserByIdAsync(userId);
        }

        public Task<User?> GetUserByUsernameAsync(string userName)
        {
            return _userRepository.GetUserByUsernameAsync(userName);
        }

        public Task UpdateUserAsync(User user)
        {
            return _userRepository.UpdateUserAsync(user);
        }
    }
}
