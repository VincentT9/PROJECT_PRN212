﻿using System;
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
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();

                user.Blogs = new List<Blog>();
                user.MedicalConsultations = new List<MedicalConsultation>();
                user.MedicalIncidents = new List<MedicalIncident>();
                user.MedicationRequests = new List<MedicationRequest>();
                user.Students = new List<Student>();
                user.Campaigns = new List<Campaign>();

                if (user.Id == Guid.Empty)
                {
                    user.Id = Guid.NewGuid();
                }

                user.CreateAt = DateTime.SpecifyKind(user.CreateAt, DateTimeKind.Utc);
                user.UpdateAt = DateTime.SpecifyKind(user.UpdateAt, DateTimeKind.Utc);

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");

                throw; 
            }
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");

                throw;
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var listUser = await _context.Users
                    .AsNoTracking()
                    .ToListAsync();
                return listUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all users: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");

                throw;
            }
        }

        public async Task<User?> GetUserByUsernameAsync(string userName)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Username == userName);
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user by username: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");

                throw;
            }
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userId);
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user by ID: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");

                throw;
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();

                var existingUser = await _context.Users.FindAsync(user.Id);
                if (existingUser == null)
                {
                    throw new KeyNotFoundException($"User with ID {user.Id} not found");
                }

                existingUser.Username = user.Username;
                existingUser.Password = user.Password;
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.Address = user.Address;
                existingUser.UserRole = user.UserRole;
                existingUser.Image = user.Image;
                existingUser.UpdatedBy = user.UpdatedBy;

                existingUser.UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");

                throw;
            }
        }

        public async Task<List<User>> GetUsersByRoleAsync(int role)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var users = await _context.Users
                    .AsNoTracking()
                    .Where(u => u.UserRole == role)
                    .ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting users by role: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }
    }
}
