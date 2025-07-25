using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using BusinessObjects;
using SchoolMedicalManagementSystem.Enum;
using Services;
using Services.Interface;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class RegisterWindow : Window
    {
        private readonly IUserService _userService;
        
        public RegisterWindow()
        {
            InitializeComponent();
            _userService = new UserService();
            
            // Set focus to username textbox
            txtUsername.Focus();
        }
        
        private void txtLogin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Chuyển về trang đăng nhập
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async void btnRegister_Click(object sender, RoutedEventArgs e)
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
                
                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("Vui lòng nhập email!", "Thông báo", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtEmail.Focus();
                    return;
                }
                
                if (!IsValidEmail(txtEmail.Text.Trim()))
                {
                    MessageBox.Show("Email không đúng định dạng!", "Thông báo", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtEmail.Focus();
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtPassword.Password))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPassword.Focus();
                    return;
                }
                
                if (txtPassword.Password.Length < 6)
                {
                    MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPassword.Focus();
                    return;
                }
                
                if (txtPassword.Password != txtConfirmPassword.Password)
                {
                    MessageBox.Show("Xác nhận mật khẩu không khớp!", "Thông báo", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtConfirmPassword.Focus();
                    return;
                }
                
                // Disable button để tránh click nhiều lần
                btnRegister.IsEnabled = false;
                btnRegister.Content = "Đang đăng ký...";
                
                // Kiểm tra username đã tồn tại chưa
                var existingUser = await _userService.GetUserByUsernameAsync(txtUsername.Text.Trim());
                if (existingUser != null)
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại, vui lòng chọn tên khác!", "Thông báo", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtUsername.Focus();
                    return;
                }
                
                // Tạo user mới
                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Username = txtUsername.Text.Trim(),
                    Password = HashPasswordToSha256(txtPassword.Password),
                    Email = txtEmail.Text.Trim(),
                    FullName = txtUsername.Text.Trim(), // Mặc định dùng username làm fullname
                    UserRole = (int)UserRole.Parent, // Mặc định là tài khoản phụ huynh
                    CreateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                    UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                    CreatedBy = "System",
                    UpdatedBy = "System"
                };
                
                // Lưu vào database
                await _userService.AddUserAsync(newUser);
                
                // Đăng ký thành công
                MessageBox.Show("Đăng ký tài khoản thành công!\nVui lòng đăng nhập để tiếp tục.", 
                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Chuyển về trang đăng nhập
                var loginWindow = new LoginWindow();
                loginWindow.Show();
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
                btnRegister.IsEnabled = true;
                btnRegister.Content = "Đăng ký";
            }
        }
        
        private bool IsValidEmail(string email)
        {
            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }
        
        // Method hash password sử dụng SHA256
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