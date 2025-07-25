using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using BusinessObjects;
using Services.Interface;
using Services;
using SchoolMedicalManagementSystem.Enum;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class LoginWindow : Window
    {
        private readonly IUserService _userService;
        public static User? CurrentUser { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
            _userService = new UserService();
            
            // Set focus to username textbox
            txtUsername.Focus();
            
            // Handle Enter key press
            KeyDown += LoginWindow_KeyDown;
        }

        private void LoginWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Click(sender, new RoutedEventArgs());
            }
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra input
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtUsername.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Password))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPassword.Focus();
                    return;
                }

                // Disable button để tránh click nhiều lần
                btnLogin.IsEnabled = false;
                btnLogin.Content = "Đang đăng nhập...";

                // Tìm user trong database
                var user = await _userService.GetUserByUsernameAsync(txtUsername.Text.Trim());

                if (user == null)
                {
                    MessageBox.Show("Tên đăng nhập không tồn tại!", "Lỗi đăng nhập", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Kiểm tra mật khẩu với SHA256 hash
                var hashedPassword = HashPasswordToSha256(txtPassword.Password);
                if (user.Password != hashedPassword)
                {
                    MessageBox.Show("Mật khẩu không chính xác!", "Lỗi đăng nhập", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Đăng nhập thành công
                CurrentUser = user;
                
                MessageBox.Show($"Đăng nhập thành công!\nChào mừng {user.FullName}", 
                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                // Mở AdminDashboard và đóng LoginWindow
                var adminDashboard = new AdminDashboard();
                adminDashboard.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Enable lại button
                btnLogin.IsEnabled = true;
                btnLogin.Content = "Đăng nhập";
            }
        }

        // Method để logout
        public static void Logout()
        {
            CurrentUser = null;
        }

        // Method kiểm tra quyền admin
        public static bool IsAdmin()
        {
            return CurrentUser?.UserRole == UserRole.Admin;
        }

        // Method kiểm tra đã đăng nhập chưa
        public static bool IsLoggedIn()
        {
            return CurrentUser != null;
        }

        // Method hash password sử dụng SHA256 (giống với hệ thống web)
        private string HashPasswordToSha256(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}