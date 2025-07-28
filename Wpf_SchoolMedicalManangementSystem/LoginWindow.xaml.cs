using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using BusinessObjects;
using Services.Interface;
using Services;
using SchoolMedicalManagementSystem.Enum;
using System.IO;
using System.Configuration;

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

            // Load saved login information
            LoadSavedLoginInfo();
        }

        private void LoadSavedLoginInfo()
        {
            try
            {
                string appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "FPTMed");
                
                if (!Directory.Exists(appDataPath))
                {
                    return; 
                }

                string rememberMeFile = Path.Combine(appDataPath, "rememberme.txt");
                if (File.Exists(rememberMeFile))
                {
                    string[] lines = File.ReadAllLines(rememberMeFile);
                    if (lines.Length >= 2 && lines[0] == "true")
                    {
                        txtUsername.Text = lines[1];
                        chkRememberMe.IsChecked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                
                System.Diagnostics.Debug.WriteLine($"Error loading remembered login: {ex.Message}");
            }
        }

        private void SaveLoginInfo()
        {
            try
            {
                string appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "FPTMed");
                
                // Create directory if it doesn't exist
                if (!Directory.Exists(appDataPath))
                {
                    Directory.CreateDirectory(appDataPath);
                }

                string rememberMeFile = Path.Combine(appDataPath, "rememberme.txt");

                // If remember me is checked, save the username
                if (chkRememberMe.IsChecked == true)
                {
                    File.WriteAllLines(rememberMeFile, new[] 
                    { 
                        "true",
                        txtUsername.Text.Trim()
                    });
                }
                else
                {
                    // If not checked, remove the file if it exists
                    if (File.Exists(rememberMeFile))
                    {
                        File.Delete(rememberMeFile);
                    }
                }
            }
            catch (Exception ex)
            {
                
                System.Diagnostics.Debug.WriteLine($"Error saving login info: {ex.Message}");
            }
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

                SaveLoginInfo();

                // Đăng nhập thành công
                CurrentUser = user;
                
                MessageBox.Show($"Đăng nhập thành công!\nChào mừng {user.FullName}", 
                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                if(user.UserRole == 0 || user.UserRole == 2)
                {
                    App.Current.Properties["CurrentUser"] = CurrentUser;
                    AdminDashboard adminDashboard = new AdminDashboard();
                    adminDashboard.Show();
                    this.Close();
                }else if(user.UserRole == 1)
                {
                    App.Current.Properties["CurrentUser"] = CurrentUser;
                    ParentDashboar parentWindow = new ParentDashboar();
                    parentWindow.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                btnLogin.IsEnabled = true;
                btnLogin.Content = "Đăng nhập";
            }
        }

        private void txtRegister_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var registerWindow = new RegisterWindow();
                registerWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void Logout()
        {
            CurrentUser = null;
            
            try
            {
                
                string appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "FPTMed");
                
                string rememberMeFile = Path.Combine(appDataPath, "rememberme.txt");
                if (File.Exists(rememberMeFile))
                {
                    File.Delete(rememberMeFile);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing credentials: {ex.Message}");
            }
        }

        // Method kiểm tra quyền admin
        public static bool IsAdmin()
        {
            return CurrentUser?.UserRole == (int)UserRole.Admin;
        }
        
        // Method kiểm tra quyền y tá
        public static bool IsMedicalStaff()
        {
            return CurrentUser?.UserRole == (int)UserRole.MedicalStaff;
        }

        //Method kiểm tra quyền có phải nhân viên y tế không ? 
        public static bool IsNurse()
        {
            return CurrentUser?.UserRole == (int)UserRole.MedicalStaff;
        }
        //Method kiểm tra quyền có phải  là phụ huynh hay không ? 
        public static bool isParent()
        {
            return CurrentUser?.UserRole == (int)UserRole.Parent;
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