using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface INotificationService
    {
        Task<List<Notification>> GetAllNotificationsAsync();
        Task<Notification?> GetNotificationByIdAsync(Guid notificationId);
        Task<List<Notification>> GetNotificationsByUserIdAsync(Guid userId);
        Task CreateNotificationAsync(Notification notification);
        Task UpdateNotificationAsync(Notification notification);
        Task DeleteNotificationAsync(Guid notificationId);
        Task AssignNotificationToUserAsync(Guid notificationId, Guid userId);
        Task AssignNotificationToAllUsersAsync(Guid notificationId);
        Task RemoveNotificationFromUserAsync(Guid notificationId, Guid userId);
    }
} 