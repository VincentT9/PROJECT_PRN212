using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using SchoolMedicalManagementSystem.Enum;
using Services;
using Services.Interface;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class UserManagementView : Page
    {
        private readonly IUserService _userService;
        private List<UserDisplayModel> _users;
        private UserDisplayModel? _selectedUser;
        private bool _isEditMode;

        public UserManagementView()
        {
            InitializeComponent();
            _userService = new UserService();
            _users = new List<UserDisplayModel>();
            LoadUsers();
            _ = LoadUsers();
        }

        private async Task LoadUsers()
        {
            try
            {
                txtStatus.Text = "Đang tải dữ liệu...";
                
                var users = await _userService.GetAllUsersAsync();
                _users = users.Select(u => new UserDisplayModel
                {
                    Id = u.Id,
                    Username = u.Username ?? "",
                    FullName = u.FullName ?? "",
                    Email = u.Email ?? "",
                    PhoneNumber = u.PhoneNumber ?? "",
                    Address = u.Address ?? "",
                    UserRole = (UserRole)u.UserRole,
                    UserRoleDisplay = GetRoleDisplayName((UserRole)u.UserRole),
                    CreateAt = u.CreateAt,
                    UpdateAt = u.UpdateAt,
                    Password = u.Password ?? ""
                }).ToList();

                dgUsers.ItemsSource = _users;
                txtStatus.Text = $"Đã tải {_users.Count} người dùng";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "Lỗi khi tải dữ liệu";
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ShowUserForm(false);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            _selectedUser = dgUsers.SelectedItem as UserDisplayModel;
            if (_selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng cần sửa!", "Thông báo", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ShowUserForm(true);
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            _selectedUser = dgUsers.SelectedItem as UserDisplayModel;
            if (_selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng cần xóa!", "Thông báo", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Không cho phép xóa chính mình
            if (_selectedUser.Id == LoginWindow.CurrentUser?.Id)
            {
                MessageBox.Show("Không thể xóa tài khoản đang đăng nhập!", "Cảnh báo", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa người dùng '{_selectedUser.FullName}'?", 
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    txtStatus.Text = "Đang xóa người dùng...";
                    await _userService.DeleteUserAsync(_selectedUser.Id);
                    
                    MessageBox.Show("Xóa người dùng thành công!", "Thành công", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    await LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa người dùng: {ex.Message}", "Lỗi", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    txtStatus.Text = "Lỗi khi xóa người dùng";
                }
            }
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await LoadUsers();
        }

        private void ShowUserForm(bool isEdit)
        {
            _isEditMode = isEdit;
            
            if (isEdit)
            {
                txtFormTitle.Text = "Sửa thông tin người dùng";
                lblPassword.Text = "Mật khẩu";
                lblPasswordNote.Visibility = Visibility.Visible;
                FillFormData(_selectedUser!);
            }
            else
            {
                txtFormTitle.Text = "Thêm người dùng mới";
                lblPassword.Text = "Mật khẩu *";
                lblPasswordNote.Visibility = Visibility.Collapsed;
                ClearFormData();
            }

            UserFormOverlay.Visibility = Visibility.Visible;
            txtFormUsername.Focus();
        }

        private void FillFormData(UserDisplayModel user)
        {
            txtFormUsername.Text = user.Username;
            txtFormUsername.IsEnabled = false; // Không cho sửa username
            txtFormPassword.Password = ""; // Không hiển thị password cũ
            txtFormFullName.Text = user.FullName;
            txtFormEmail.Text = user.Email;
            txtFormPhone.Text = user.PhoneNumber;
            txtFormAddress.Text = user.Address;

            // Set role
            cmbFormRole.SelectedIndex = (int)user.UserRole;
        }

        private void ClearFormData()
        {
            txtFormUsername.Text = "";
            txtFormUsername.IsEnabled = true;
            txtFormPassword.Password = "";
            txtFormFullName.Text = "";
            txtFormEmail.Text = "";
            txtFormPhone.Text = "";
            txtFormAddress.Text = "";
            cmbFormRole.SelectedIndex = -1;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                txtStatus.Text = _isEditMode ? "Đang cập nhật..." : "Đang thêm mới...";

                var user = new User
                {
                    Username = txtFormUsername.Text.Trim(),
                    Password = HashPasswordToSha256(txtFormPassword.Password),
                    FullName = txtFormFullName.Text.Trim(),
                    Email = txtFormEmail.Text.Trim(),
                    PhoneNumber = txtFormPhone.Text.Trim(),
                    Address = txtFormAddress.Text.Trim(),
                    UserRole = (int)(UserRole)cmbFormRole.SelectedIndex,
                    CreatedBy = LoginWindow.CurrentUser?.Username ?? "System",
                    UpdatedBy = LoginWindow.CurrentUser?.Username ?? "System"
                };

                if (_isEditMode)
                {
                    user.Id = _selectedUser!.Id;
                    user.CreateAt = _selectedUser.CreateAt;
                    // Chỉ hash và cập nhật password nếu có password mới được nhập
                    if (!string.IsNullOrWhiteSpace(txtFormPassword.Password))
                    {
                        user.Password = HashPasswordToSha256(txtFormPassword.Password);
                    }
                    else
                    {
                        // Giữ password cũ
                        user.Password = _selectedUser.Password;
                    }
                    await _userService.UpdateUserAsync(user);
                    MessageBox.Show("Cập nhật người dùng thành công!", "Thành công", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Khi thêm mới, luôn hash password
                    user.Password = HashPasswordToSha256(txtFormPassword.Password);
                    await _userService.AddUserAsync(user);
                    MessageBox.Show("Thêm người dùng thành công!", "Thành công", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                UserFormOverlay.Visibility = Visibility.Collapsed;
                await LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "Lỗi khi lưu dữ liệu";
            }
        }

        private bool ValidateForm()
        {
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(txtFormUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFormUsername.Focus();
                return false;
            }

            // Chỉ kiểm tra password khi thêm mới, khi edit có thể để trống để giữ password cũ
            if (!_isEditMode && string.IsNullOrWhiteSpace(txtFormPassword.Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFormPassword.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFormFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFormFullName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFormEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFormEmail.Focus();
                return false;
            }

            if (cmbFormRole.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn vai trò!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbFormRole.Focus();
                return false;
            }

            // Kiểm tra format email
            if (!IsValidEmail(txtFormEmail.Text.Trim()))
            {
                MessageBox.Show("Email không đúng định dạng!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFormEmail.Focus();
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            UserFormOverlay.Visibility = Visibility.Collapsed;
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

    // Model để hiển thị trên DataGrid
    public class UserDisplayModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public UserRole UserRole { get; set; }
        public string UserRoleDisplay { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}