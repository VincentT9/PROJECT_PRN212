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
        public async Task AddUserAsync(User user)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            var listUser = await context.Users
                .Include(u => u.Students!)
                .AsNoTracking()
                .ToListAsync();
            return listUser;
        }

        public async Task<User?> GetUserByUsernameAsync(string userName)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == userName);
            return user;
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            var user = await context.Users
                 .Include(u => u.Students!)
                 .ThenInclude(u => u.HealthRecord!)
                .FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<List<User>> GetUsersByRoleAsync(int role)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            var users = await context.Users
                .Where(u => u.UserRole == role)
                .Include(u => u.Students!)
                .AsNoTracking()
                .ToListAsync();
            return users;
        }
    }
}
