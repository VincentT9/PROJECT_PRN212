using BusinessObjects;
using Services;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class ParentNotificationView : UserControl
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IMedicalIncidentService _medicalIncidentService;
        private User? currentUser;
        public ObservableCollection<Notification> Notifications { get; set; }

        public ParentNotificationView()
        {
            InitializeComponent();
            _notificationService = new NotificationService();
            _userService = new UserService();
            _medicalIncidentService = new MedicalIncidentService();
            Notifications = new ObservableCollection<Notification>();
            this.DataContext = this;
            LoadParentNotificationsAsync();
        }

        private async void LoadParentNotificationsAsync()
        {
            currentUser = App.Current.Properties["CurrentUser"] as BusinessObjects.User;
            if (currentUser == null)
            {
                txtStatus.Text = "Không tìm thấy thông tin người dùng. Vui lòng đăng nhập lại.";
                return;
            }
            // Lấy danh sách thông báo dành cho phụ huynh này
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(currentUser.Id);
            Notifications.Clear();
            foreach (var n in notifications)
            {
                Notifications.Add(n);
            }
            lvNotifications.ItemsSource = Notifications;
            txtStatus.Text = $"Có {Notifications.Count} thông báo.";
        }

        private async void lvNotifications_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvNotifications.SelectedItem is Notification notification)
            {
                string incidentIdStr = null;
                if (!string.IsNullOrEmpty(notification.ReturnUrl))
                {
                    if (notification.ReturnUrl.StartsWith("incident:"))
                        incidentIdStr = notification.ReturnUrl.Substring("incident:".Length);
                    else
                        incidentIdStr = notification.ReturnUrl;
                }

                if (!string.IsNullOrEmpty(incidentIdStr) && Guid.TryParse(incidentIdStr, out Guid incidentId))
                {
                    var incident = await _medicalIncidentService.GetMedicalIncidentByIdAsync(incidentId);
                    if (incident != null)
                    {
                        var detailView = new MedicalIncidentDetailView(incident);
                        detailView.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy chi tiết sự kiện y tế.");
                    }
                }
            }
        }
    }
} 