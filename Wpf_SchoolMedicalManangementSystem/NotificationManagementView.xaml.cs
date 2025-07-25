using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using SchoolMedicalManagementSystem.Enum;
using Services;
using Services.Interface;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class NotificationManagementView : Page
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private List<Notification> _notifications;
        private Notification? _selectedNotification;
        private bool _isEditMode;

        public NotificationManagementView()
        {
            InitializeComponent();
            _notificationService = new NotificationService();
            _userService = new UserService();
            _notifications = new List<Notification>();
            
            LoadNotifications();
        }

        private async void LoadNotifications()
        {
            try
            {
                txtStatus.Text = "Đang tải dữ liệu...";
                
                // Lấy danh sách thông báo
                _notifications = await _notificationService.GetAllNotificationsAsync();
                dgNotifications.ItemsSource = _notifications;
                
                txtStatus.Text = $"{_notifications.Count} thông báo";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "Lỗi khi tải dữ liệu";
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var searchText = txtSearch.Text.ToLower().Trim();
                
                if (string.IsNullOrEmpty(searchText))
                {
                    dgNotifications.ItemsSource = _notifications;
                }
                else
                {
                    var filteredNotifications = _notifications.Where(n =>
                        n.Title.ToLower().Contains(searchText) ||
                        n.Content.ToLower().Contains(searchText)
                    ).ToList();
                    
                    dgNotifications.ItemsSource = filteredNotifications;
                    txtStatus.Text = $"Hiển thị {filteredNotifications.Count}/{_notifications.Count} thông báo";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ShowNotificationForm(false);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is Notification notification)
            {
                _selectedNotification = notification;
                ShowNotificationForm(true);
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                if (button?.DataContext is Notification notification)
                {
                    // Xác nhận xóa
                    var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa thông báo '{notification.Title}'?", 
                        "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    
                    if (result == MessageBoxResult.Yes)
                    {
                        await _notificationService.DeleteNotificationAsync(notification.Id);
                        MessageBox.Show("Xóa thông báo thành công!", "Thành công", 
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadNotifications();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa thông báo: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadNotifications();
        }

        private void dgNotifications_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedNotification = dgNotifications.SelectedItem as Notification;
        }

        private void ShowNotificationForm(bool isEdit)
        {
            _isEditMode = isEdit;
            
            if (isEdit)
            {
                // Hiển thị form sửa thông báo
                txtFormTitle.Text = _selectedNotification.Title;
                txtFormContent.Text = _selectedNotification.Content;
                txtFormReturnUrl.Text = _selectedNotification.ReturnUrl;
                
                NotificationFormOverlay.Visibility = Visibility.Visible;
                this.lblFormTitle.Text = "Sửa thông báo";
                txtFormTitle.Focus();
            }
            else
            {
                // Hiển thị form thêm thông báo
                txtFormTitle.Text = "";
                txtFormContent.Text = "";
                txtFormReturnUrl.Text = "";
                
                NotificationFormOverlay.Visibility = Visibility.Visible;
                this.lblFormTitle.Text = "Thêm thông báo mới";
                txtFormTitle.Focus();
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate form
                if (string.IsNullOrWhiteSpace(txtFormTitle.Text))
                {
                    MessageBox.Show("Vui lòng nhập tiêu đề thông báo!", "Lỗi", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtFormTitle.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFormContent.Text))
                {
                    MessageBox.Show("Vui lòng nhập nội dung thông báo!", "Lỗi", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtFormContent.Focus();
                    return;
                }

                // Disable button để tránh click nhiều lần
                btnSave.IsEnabled = false;
                btnSave.Content = "Đang lưu...";

                if (_isEditMode)
                {
                    // Cập nhật thông báo hiện tại
                    _selectedNotification.Title = txtFormTitle.Text;
                    _selectedNotification.Content = txtFormContent.Text;
                    _selectedNotification.ReturnUrl = txtFormReturnUrl.Text;
                    _selectedNotification.UpdatedBy = LoginWindow.CurrentUser?.Username ?? "System";
                    
                    await _notificationService.UpdateNotificationAsync(_selectedNotification);
                    
                    MessageBox.Show("Cập nhật thông báo thành công!", "Thành công", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Tạo thông báo mới
                    var newNotification = new Notification
                    {
                        Id = Guid.NewGuid(),
                        Title = txtFormTitle.Text,
                        Content = txtFormContent.Text,
                        ReturnUrl = txtFormReturnUrl.Text,
                        CreatedBy = LoginWindow.CurrentUser?.Username ?? "System",
                        UpdatedBy = LoginWindow.CurrentUser?.Username ?? "System"
                    };
                    
                    await _notificationService.CreateNotificationAsync(newNotification);
                    
                    MessageBox.Show("Thêm thông báo thành công!", "Thành công", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Đóng form và cập nhật danh sách
                NotificationFormOverlay.Visibility = Visibility.Collapsed;
                LoadNotifications();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Enable lại button
                btnSave.IsEnabled = true;
                btnSave.Content = "💾 Lưu thông báo";
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NotificationFormOverlay.Visibility = Visibility.Collapsed;
        }

        // Phương thức này đã bị vô hiệu hóa do chức năng quản lý người dùng đã được gỡ bỏ
        private void btnAssign_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng quản lý người dùng nhận thông báo đã được gỡ bỏ.", 
                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private string GetRoleDisplayName(UserRole role)
        {
            return role switch
            {
                UserRole.Admin => "Quản trị viên",
                UserRole.Parent => "Phụ huynh",
                UserRole.MedicalStaff => "Y tá",
                _ => "Không xác định"
            };
        }
    }

    public class UserViewModel : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = "";
        public string FullName { get; set; } = "";
        public int UserRole { get; set; }
        public string UserRoleDisplay { get; set; } = "";
        
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 