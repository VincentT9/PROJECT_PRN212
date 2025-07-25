using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using SchoolMedicalManagementSystem.Enum;
using Services;
using Services.Interface;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class ProfileView : Page
    {
        private readonly IUserService _userService;
        private User? _currentUser;
        private bool _isEditMode = false;

        public ProfileView()
        {
            InitializeComponent();
            _userService = new UserService();
            
            LoadUserProfile();
        }

        private async void LoadUserProfile()
        {
            try
            {
                if (LoginWindow.CurrentUser == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin người dùng!", "Lỗi", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Load lại thông tin user từ database để đảm bảo dữ liệu mới nhất
                _currentUser = await _userService.GetUserByIdAsync(LoginWindow.CurrentUser.Id);
                
                if (_currentUser == null)
                {
                    MessageBox.Show("Không thể tải thông tin người dùng!", "Lỗi", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Hiển thị thông tin lên giao diện
                DisplayUserInfo();
                SetEditMode(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayUserInfo()
        {
            if (_currentUser == null) return;

            // Hiển thị thông tin cơ bản
            txtDisplayName.Text = $"@{_currentUser.Username}";
            txtUserRole.Text = GetRoleDisplayName((UserRole)_currentUser.UserRole);
            txtLastUpdate.Text = $"Cập nhật lần cuối: {_currentUser.UpdateAt:dd/MM/yyyy HH:mm}";

            // Fill form fields
            txtUsername.Text = _currentUser.Username ?? "";
            txtFullName.Text = _currentUser.FullName ?? "";
            txtEmail.Text = _currentUser.Email ?? "";
            txtPhoneNumber.Text = _currentUser.PhoneNumber ?? "";
            txtAddress.Text = _currentUser.Address ?? "";
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

        private void SetEditMode(bool isEdit)
        {
            _isEditMode = isEdit;
            
            // Enable/disable controls
            txtFullName.IsReadOnly = !isEdit;
            txtEmail.IsReadOnly = !isEdit;
            txtPhoneNumber.IsReadOnly = !isEdit;
            txtAddress.IsReadOnly = !isEdit;

            // Change styles
            if (isEdit)
            {
                txtFullName.Style = (Style)FindResource("ModernTextBox");
                txtEmail.Style = (Style)FindResource("ModernTextBox");
                txtPhoneNumber.Style = (Style)FindResource("ModernTextBox");
                txtAddress.Style = (Style)FindResource("ModernTextBox");
                btnEdit.Content = "❌ Hủy";
            }
            else
            {
                txtFullName.Style = (Style)FindResource("ReadOnlyTextBox");
                txtEmail.Style = (Style)FindResource("ReadOnlyTextBox");
                txtPhoneNumber.Style = (Style)FindResource("ReadOnlyTextBox");
                txtAddress.Style = (Style)FindResource("ReadOnlyTextBox");
                btnEdit.Content = "✏️ Chỉnh sửa";
            }

            // Show/hide save button
            btnSave.Visibility = isEdit ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_isEditMode)
            {
                // Cancel edit - reload original data
                DisplayUserInfo();
                SetEditMode(false);
            }
            else
            {
                // Start edit mode
                SetEditMode(true);
                txtFullName.Focus();
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateUserInfo())
                return;

            try
            {
                if (_currentUser == null) return;

                // Update user object
                _currentUser.FullName = txtFullName.Text.Trim();
                _currentUser.Email = txtEmail.Text.Trim();
                _currentUser.PhoneNumber = txtPhoneNumber.Text.Trim();
                _currentUser.Address = txtAddress.Text.Trim();
                _currentUser.UpdatedBy = _currentUser.Username ?? "System";

                // Save to database
                await _userService.UpdateUserAsync(_currentUser);

                // Update current user in session
                LoginWindow.CurrentUser.FullName = _currentUser.FullName;
                LoginWindow.CurrentUser.Email = _currentUser.Email;
                LoginWindow.CurrentUser.PhoneNumber = _currentUser.PhoneNumber;
                LoginWindow.CurrentUser.Address = _currentUser.Address;

                MessageBox.Show("Cập nhật thông tin thành công!", "Thành công", 
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // Refresh display and exit edit mode
                DisplayUserInfo();
                SetEditMode(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateUserInfo()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFullName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return false;
            }

            if (!IsValidEmail(txtEmail.Text.Trim()))
            {
                MessageBox.Show("Email không đúng định dạng!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return false;
            }

            return true;
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

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            // Clear password fields
            txtCurrentPassword.Password = "";
            txtNewPassword.Password = "";
            txtConfirmPassword.Password = "";
            
            // Show modal
            ChangePasswordOverlay.Visibility = Visibility.Visible;
            txtCurrentPassword.Focus();
        }

        private async void btnSavePassword_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidatePasswordChange())
                return;

            try
            {
                if (_currentUser == null) return;

                // Verify current password
                var currentPasswordHash = HashPasswordToSha256(txtCurrentPassword.Password);
                if (_currentUser.Password != currentPasswordHash)
                {
                    MessageBox.Show("Mật khẩu hiện tại không chính xác!", "Lỗi", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    txtCurrentPassword.Focus();
                    return;
                }

                // Update password
                _currentUser.Password = HashPasswordToSha256(txtNewPassword.Password);
                _currentUser.UpdatedBy = _currentUser.Username ?? "System";

                await _userService.UpdateUserAsync(_currentUser);

                MessageBox.Show("Đổi mật khẩu thành công!", "Thành công", 
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // Close modal
                ChangePasswordOverlay.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đổi mật khẩu: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidatePasswordChange()
        {
            if (string.IsNullOrWhiteSpace(txtCurrentPassword.Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu hiện tại!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtCurrentPassword.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNewPassword.Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNewPassword.Focus();
                return false;
            }

            if (txtNewPassword.Password.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNewPassword.Focus();
                return false;
            }

            if (txtNewPassword.Password != txtConfirmPassword.Password)
            {
                MessageBox.Show("Xác nhận mật khẩu không khớp!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtConfirmPassword.Focus();
                return false;
            }

            if (txtCurrentPassword.Password == txtNewPassword.Password)
            {
                MessageBox.Show("Mật khẩu mới phải khác mật khẩu hiện tại!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNewPassword.Focus();
                return false;
            }

            return true;
        }

        private void btnCancelPassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordOverlay.Visibility = Visibility.Collapsed;
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