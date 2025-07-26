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
        private readonly IUserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddUserAsync(User user)
        {
            // Xác thực dữ liệu đầu vào
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User không được phép null");

            if (string.IsNullOrEmpty(user.Username))
                throw new ArgumentException("Username không được phép null hoặc rỗng", nameof(user));

            // Kiểm tra username đã tồn tại chưa
            var existingUser = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (existingUser != null)
                throw new InvalidOperationException($"Username '{user.Username}' đã tồn tại");

            // Thiết lập thông tin thời gian
            user.CreateAt = DateTime.Now;
            user.UpdateAt = DateTime.Now;

            // Thiết lập ID nếu chưa có
            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();

            await _userRepository.AddUserAsync(user);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            // Kiểm tra user có tồn tại không
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"Không tìm thấy user với ID: {userId}");

            await _userRepository.DeleteUserAsync(userId);
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

        public async Task UpdateUserAsync(User user)
        {
            // Xác thực dữ liệu
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User không được phép null");

            // Kiểm tra user có tồn tại không
            var existingUser = await _userRepository.GetUserByIdAsync(user.Id);
            if (existingUser == null)
                throw new KeyNotFoundException($"Không tìm thấy user với ID: {user.Id}");

            // Cập nhật thông tin thời gian
            user.UpdateAt = DateTime.Now;

            await _userRepository.UpdateUserAsync(user);
        }

        public async Task<List<User>> GetUsersByRoleAsync(SchoolMedicalManagementSystem.Enum.UserRole role)
        {
            return await _userRepository.GetUsersByRoleAsync((int)role);
        }
    }
}
