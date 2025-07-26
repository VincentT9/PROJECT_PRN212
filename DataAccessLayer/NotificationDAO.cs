using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class NotificationDAO
    {
        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                return await _context.Notifications
                    .AsNoTracking()
                    .OrderByDescending(n => n.CreateAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all notifications: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public async Task<Notification?> GetNotificationByIdAsync(Guid notificationId)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                return await _context.Notifications
                    .AsNoTracking()
                    .FirstOrDefaultAsync(n => n.Id == notificationId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting notification by ID: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public async Task<List<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var user = await _context.Users
                    .Include(u => u.Notifications)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userId);

                return user?.Notifications.OrderByDescending(n => n.CreateAt).ToList() ?? new List<Notification>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting notifications by user ID: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public async Task CreateNotificationAsync(Notification notification)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                
                // Ensure ID is set
                if (notification.Id == Guid.Empty)
                {
                    notification.Id = Guid.NewGuid();
                }
                
                // Set timestamps
                notification.CreateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                notification.UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating notification: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public async Task UpdateNotificationAsync(Notification notification)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                
                var existingNotification = await _context.Notifications.FindAsync(notification.Id);
                if (existingNotification == null)
                {
                    throw new KeyNotFoundException($"Notification with ID {notification.Id} not found");
                }
                
                // Update properties
                existingNotification.Title = notification.Title;
                existingNotification.Content = notification.Content;
                existingNotification.ReturnUrl = notification.ReturnUrl;
                existingNotification.UpdatedBy = notification.UpdatedBy;
                existingNotification.UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating notification: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public async Task DeleteNotificationAsync(Guid notificationId)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                
                var notification = await _context.Notifications.FindAsync(notificationId);
                if (notification != null)
                {
                    _context.Notifications.Remove(notification);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting notification: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public async Task AssignNotificationToUserAsync(Guid notificationId, Guid userId)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                
                var notification = await _context.Notifications
                    .Include(n => n.Users)
                    .FirstOrDefaultAsync(n => n.Id == notificationId);
                    
                var user = await _context.Users.FindAsync(userId);
                
                if (notification == null || user == null)
                {
                    throw new KeyNotFoundException("Notification or User not found");
                }
                
                if (!notification.Users.Any(u => u.Id == userId))
                {
                    notification.Users.Add(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning notification to user: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public async Task AssignNotificationToAllUsersAsync(Guid notificationId)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                
                var notification = await _context.Notifications
                    .Include(n => n.Users)
                    .FirstOrDefaultAsync(n => n.Id == notificationId);
                    
                var users = await _context.Users.ToListAsync();
                
                if (notification == null)
                {
                    throw new KeyNotFoundException("Notification not found");
                }
                
                foreach (var user in users)
                {
                    if (!notification.Users.Any(u => u.Id == user.Id))
                    {
                        notification.Users.Add(user);
                    }
                }
                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning notification to all users: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public async Task RemoveNotificationFromUserAsync(Guid notificationId, Guid userId)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                
                var notification = await _context.Notifications
                    .Include(n => n.Users)
                    .FirstOrDefaultAsync(n => n.Id == notificationId);
                    
                if (notification == null)
                {
                    throw new KeyNotFoundException("Notification not found");
                }
                
                var user = notification.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    notification.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing notification from user: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }
    }
} 