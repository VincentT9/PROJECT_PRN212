using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class UserDAO
    {
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var listUser = await _context.Users
                .Include(u => u.Students!)
                .AsNoTracking()
                .ToListAsync();
            return listUser;
        }

        public async Task<User?> GetUserByUsernameAsync(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
            return user;
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            var user = await _context.Users
                 .Include(u => u.Students!)
                 .ThenInclude(u => u.HealthRecord!)
                .FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
