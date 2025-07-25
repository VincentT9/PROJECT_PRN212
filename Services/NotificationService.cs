using BusinessObjects;
using Repositories;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService()
        {
            _notificationRepository = new NotificationRepository();
        }

        public Task<List<Notification>> GetAllNotificationsAsync()
        {
            return _notificationRepository.GetAllNotificationsAsync();
        }

        public Task<Notification?> GetNotificationByIdAsync(Guid notificationId)
        {
            return _notificationRepository.GetNotificationByIdAsync(notificationId);
        }

        public Task<List<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            return _notificationRepository.GetNotificationsByUserIdAsync(userId);
        }

        public async Task CreateNotificationAsync(Notification notification)
        {
            // Validate notification
            if (string.IsNullOrWhiteSpace(notification.Title))
            {
                throw new ArgumentException("Notification title cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(notification.Content))
            {
                throw new ArgumentException("Notification content cannot be empty");
            }

            await _notificationRepository.CreateNotificationAsync(notification);
        }

        public async Task UpdateNotificationAsync(Notification notification)
        {
            // Validate notification
            if (notification.Id == Guid.Empty)
            {
                throw new ArgumentException("Notification ID must be specified");
            }

            if (string.IsNullOrWhiteSpace(notification.Title))
            {
                throw new ArgumentException("Notification title cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(notification.Content))
            {
                throw new ArgumentException("Notification content cannot be empty");
            }

            await _notificationRepository.UpdateNotificationAsync(notification);
        }

        public Task DeleteNotificationAsync(Guid notificationId)
        {
            if (notificationId == Guid.Empty)
            {
                throw new ArgumentException("Notification ID must be specified");
            }

            return _notificationRepository.DeleteNotificationAsync(notificationId);
        }

        public Task AssignNotificationToUserAsync(Guid notificationId, Guid userId)
        {
            if (notificationId == Guid.Empty || userId == Guid.Empty)
            {
                throw new ArgumentException("Notification ID and User ID must be specified");
            }

            return _notificationRepository.AssignNotificationToUserAsync(notificationId, userId);
        }

        public Task AssignNotificationToAllUsersAsync(Guid notificationId)
        {
            if (notificationId == Guid.Empty)
            {
                throw new ArgumentException("Notification ID must be specified");
            }

            return _notificationRepository.AssignNotificationToAllUsersAsync(notificationId);
        }

        public Task RemoveNotificationFromUserAsync(Guid notificationId, Guid userId)
        {
            if (notificationId == Guid.Empty || userId == Guid.Empty)
            {
                throw new ArgumentException("Notification ID and User ID must be specified");
            }

            return _notificationRepository.RemoveNotificationFromUserAsync(notificationId, userId);
        }
    }
} 