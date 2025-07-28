using System.Windows;
using System.Windows.Controls;
using SchoolMedicalManagementSystem.Enum;
using System;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class AdminDashboard : Window
    {
        private bool _isLogout = false;

        public AdminDashboard()
        {
            InitializeComponent();
            
            // Kiểm tra quyền truy cập
            if (!LoginWindow.IsLoggedIn())
            {
                MessageBox.Show("Vui lòng đăng nhập trước!", "Thông báo", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
                return;
            }

            // Cập nhật thông tin user
            UpdateUserInfo();
            
            // Load trang dashboard mặc định
            LoadDashboardHome();
        }

        private void UpdateUserInfo()
        {
            var user = LoginWindow.CurrentUser;
            if (user != null)
            {
                txtWelcome.Text = $"Chào mừng {user.FullName} ({GetRoleDisplayName((UserRole)user.UserRole)})";
            }
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

        private void LoadDashboardHome()
        {
            // Tạo trang dashboard chính với thống kê tổng quan
            var dashboardPage = new Page();
            var grid = new Grid();
            
            // Header
            var user = LoginWindow.CurrentUser;
            var welcomeText = user != null ? $"Dashboard - Chào mừng {user.FullName}!" : "Dashboard - Tổng quan hệ thống";
            
            var headerText = new TextBlock
            {
                Text = welcomeText,
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(30, 30, 30, 20),
                Foreground = System.Windows.Media.Brushes.DarkBlue
            };
            
            // Thông tin thống kê
            var statsPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(30, 0, 30, 30)
            };

            // Tạo các card thống kê
            var welcomeCard = CreateStatsCard("Chào mừng đến với FPTMED", 
                "Hệ thống quản lý sức khỏe học đường", "#FF2E86AB");
            
            var userCard = CreateStatsCard("Quản lý người dùng", 
                "Quản lý tài khoản và phân quyền người dùng", "#FF5CB85C");
            
            var studentCard = CreateStatsCard("Quản lý học sinh", 
                "Quản lý thông tin và hồ sơ sức khỏe học sinh", "#FFFF9800");

            var profileCard = CreateStatsCard("Trang cá nhân", 
                "Quản lý thông tin cá nhân và cài đặt tài khoản", "#FF9C27B0");
                
            var vaccinationCard = CreateStatsCard("Chương trình Tiêm chủng", 
                "Quản lý chương trình tiêm chủng và khám sức khỏe", "#FF17A2B8");

            statsPanel.Children.Add(welcomeCard);
            statsPanel.Children.Add(userCard);
            statsPanel.Children.Add(studentCard);
            statsPanel.Children.Add(vaccinationCard);
            statsPanel.Children.Add(profileCard);

            grid.Children.Add(new StackPanel
            {
                Children = { headerText, statsPanel }
            });

            dashboardPage.Content = grid;
            MainFrame.Content = dashboardPage;
        }

        private Border CreateStatsCard(string title, string description, string color)
        {
            var card = new Border
            {
                Background = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color)),
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(0, 10, 0, 10),
                Padding = new Thickness(20)
            };

            var stackPanel = new StackPanel();
            
            var titleText = new TextBlock
            {
                Text = title,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Foreground = System.Windows.Media.Brushes.White,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var descText = new TextBlock
            {
                Text = description,
                FontSize = 14,
                Foreground = System.Windows.Media.Brushes.White,
                TextWrapping = TextWrapping.Wrap
            };

            stackPanel.Children.Add(titleText);
            stackPanel.Children.Add(descText);
            card.Child = stackPanel;

            return card;
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            LoadDashboardHome();
        }

        private void btnUserManagement_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra quyền admin
            if (!LoginWindow.IsAdmin())
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Không có quyền", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var userManagementView = new UserManagementView();
            MainFrame.Content = userManagementView;
        }

        private void btnStudentManagement_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra quyền admin
            if (!LoginWindow.IsAdmin())
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Không có quyền", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var studentManagementView = new StudentManagementView();
            MainFrame.Content = studentManagementView;
        }
        
        private void btnVaccination_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra quyền admin hoặc y tá
            if (!LoginWindow.IsAdmin() && !LoginWindow.IsMedicalStaff())
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Không có quyền", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var vaccinationProgramView = new VaccinationProgramView();
            MainFrame.Content = vaccinationProgramView;
        }
        


        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            var profileView = new ProfileView();
            MainFrame.Content = profileView;
        }

        private void btnNotification_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra quyền admin
            if (!LoginWindow.IsAdmin() && !LoginWindow.IsMedicalStaff())
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Không có quyền", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var notificationView = new NotificationManagementView();
            MainFrame.Content = notificationView;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Xóa thông tin người dùng hiện tại
                LoginWindow.Logout();
                
                // Tắt cờ OnClosed để không tắt ứng dụng khi đóng AdminDashboard
                _isLogout = true;
                
                // Tạo và hiển thị cửa sổ đăng nhập mới
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                
                // Đóng cửa sổ AdminDashboard hiện tại
                this.Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            // Chỉ tắt ứng dụng khi không phải là đang đăng xuất
            if (!_isLogout)
            {
                Application.Current.Shutdown();
            }
            base.OnClosed(e);
        }

        private void btnMedicalEvent_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra quyền admin
            if (LoginWindow.isParent())
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Không có quyền",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var medicalEventLogView = new MedicalEventLogView();
            MainFrame.Content = medicalEventLogView;
        }
        
        private void btnMedicalSupply_Click(object sender, RoutedEventArgs e)
        {
            if(!LoginWindow.IsAdmin() && !LoginWindow.IsNurse())
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Không có quyền",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var medicalSupplyView = new MedicalSupplyView();
            MainFrame.Content = medicalSupplyView;
        }
        
        private void btnMedicalVaccination_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra quyền admin
            if (LoginWindow.isParent() || LoginWindow.IsMedicalStaff())

            var vaccineSchedule = new VaccinationProgramView();
            MainFrame.Content = vaccineSchedule;
        }

        private void btnMedicalSupplies_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra quyền admin
            if (LoginWindow.isParent())
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Không có quyền",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var medicalSuppliesView = new MedicalSuppliesInventoryView();
            MainFrame.Content = medicalSuppliesView;
        }
    }
}