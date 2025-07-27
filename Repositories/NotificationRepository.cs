using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDAO _notificationDAO;

        public NotificationRepository()
        {
            _notificationDAO = new NotificationDAO();
        }

        public Task<List<Notification>> GetAllNotificationsAsync()
        {
            return _notificationDAO.GetAllNotificationsAsync();
        }

        public Task<Notification?> GetNotificationByIdAsync(Guid notificationId)
        {
            return _notificationDAO.GetNotificationByIdAsync(notificationId);
        }

        public Task<List<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            return _notificationDAO.GetNotificationsByUserIdAsync(userId);
        }

        public Task CreateNotificationAsync(Notification notification)
        {
            return _notificationDAO.CreateNotificationAsync(notification);
        }

        public Task UpdateNotificationAsync(Notification notification)
        {
            return _notificationDAO.UpdateNotificationAsync(notification);
        }

        public Task DeleteNotificationAsync(Guid notificationId)
        {
            return _notificationDAO.DeleteNotificationAsync(notificationId);
        }

        public Task AssignNotificationToUserAsync(Guid notificationId, Guid userId)
        {
            return _notificationDAO.AssignNotificationToUserAsync(notificationId, userId);
        }

        public Task AssignNotificationToAllUsersAsync(Guid notificationId)
        {
            return _notificationDAO.AssignNotificationToAllUsersAsync(notificationId);
        }

        public Task RemoveNotificationFromUserAsync(Guid notificationId, Guid userId)
        {
            return _notificationDAO.RemoveNotificationFromUserAsync(notificationId, userId);
        }
    }
} 