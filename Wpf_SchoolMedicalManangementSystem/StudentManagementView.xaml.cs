using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using SchoolMedicalManagementSystem.Enum;
using Services;
using Services.Interface;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class StudentManagementView : Page
    {
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;
        private List<StudentDisplayModel> _students;
        private List<StudentDisplayModel> _filteredStudents;
        private List<User> _parents;
        private StudentDisplayModel? _selectedStudent;
        private bool _isEditMode;

        public StudentManagementView()
        {
            InitializeComponent();
            _studentService = new StudentService();
            _userService = new UserService();
            _students = new List<StudentDisplayModel>();
            _filteredStudents = new List<StudentDisplayModel>();
            _parents = new List<User>();
            
            // Call LoadData only once
            LoadData();
        }

        private async Task LoadData()
        {
            await LoadParents();
            await LoadStudents();
        }

        private async Task LoadParents()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                _parents = users.Where(u => u.UserRole == (int)UserRole.Parent).ToList();
                
                // Thêm option "Không có phụ huynh"
                var noParent = new User { Id = Guid.Empty, FullName = "Không có phụ huynh" };
                _parents.Insert(0, noParent);
                
                cmbFormParent.ItemsSource = _parents;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách phụ huynh: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadStudents()
        {
            try
            {
                txtStatus.Text = "Đang tải dữ liệu...";
                
                var students = await _studentService.GetAllStudentsAsync();
                _students = students.Select(s => new StudentDisplayModel
                {
                    Id = s.Id,
                    StudentCode = s.StudentCode ?? "",
                    FullName = s.FullName ?? "",
                    DateOfBirth = s.DateOfBirth,
                    Gender = (Gender)s.Gender,
                    GenderDisplay = GetGenderDisplayName((Gender)s.Gender),
                    Class = s.Class ?? "",
                    SchoolYear = s.SchoolYear ?? "",
                    ParentId = s.ParentId,
                    ParentName = GetParentName(s.ParentId),
                    CreateAt = s.CreateAt,
                    UpdateAt = s.UpdateAt
                }).ToList();

                _filteredStudents = new List<StudentDisplayModel>(_students);
                dgStudents.ItemsSource = _filteredStudents;
                txtStatus.Text = $"Đã tải {_students.Count} học sinh";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "Lỗi khi tải dữ liệu";
            }
        }

        private string GetGenderDisplayName(Gender gender)
        {
            return gender switch
            {
                Gender.Male => "Nam",
                Gender.Female => "Nữ",
                Gender.Other => "Khác",
                _ => "Không xác định"
            };
        }

        private string GetParentName(Guid? parentId)
        {
            if (parentId == null || parentId == Guid.Empty)
                return "Không có";
            
            var parent = _parents.FirstOrDefault(p => p.Id == parentId);
            return parent?.FullName ?? "Không xác định";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ShowStudentForm(false);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            _selectedStudent = dgStudents.SelectedItem as StudentDisplayModel;
            if (_selectedStudent == null)
            {
                MessageBox.Show("Vui lòng chọn học sinh cần sửa!", "Thông báo", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ShowStudentForm(true);
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            _selectedStudent = dgStudents.SelectedItem as StudentDisplayModel;
            if (_selectedStudent == null)
            {
                MessageBox.Show("Vui lòng chọn học sinh cần xóa!", "Thông báo", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa học sinh '{_selectedStudent.FullName}'?", 
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    txtStatus.Text = "Đang xóa học sinh...";
                    await _studentService.DeleteStudentAsync(_selectedStudent.Id);
                    
                    MessageBox.Show("Xóa học sinh thành công!", "Thành công", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    await LoadStudents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa học sinh: {ex.Message}", "Lỗi", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    txtStatus.Text = "Lỗi khi xóa học sinh";
                }
            }
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = txtSearch.Text.ToLower().Trim();
            
            if (string.IsNullOrEmpty(searchText))
            {
                _filteredStudents = new List<StudentDisplayModel>(_students);
            }
            else
            {
                _filteredStudents = _students.Where(s =>
                    s.StudentCode.ToLower().Contains(searchText) ||
                    s.FullName.ToLower().Contains(searchText) ||
                    s.Class.ToLower().Contains(searchText) ||
                    s.ParentName.ToLower().Contains(searchText)
                ).ToList();
            }
            
            dgStudents.ItemsSource = _filteredStudents;
            txtStatus.Text = $"Hiển thị {_filteredStudents.Count}/{_students.Count} học sinh";
        }

        private void ShowStudentForm(bool isEdit)
        {
            _isEditMode = isEdit;
            
            if (isEdit)
            {
                txtFormTitle.Text = "Sửa thông tin học sinh";
                FillFormData(_selectedStudent!);
            }
            else
            {
                txtFormTitle.Text = "Thêm học sinh mới";
                ClearFormData();
            }

            StudentFormOverlay.Visibility = Visibility.Visible;
            txtFormStudentCode.Focus();
        }

        private void FillFormData(StudentDisplayModel student)
        {
            txtFormStudentCode.Text = student.StudentCode;
            txtFormStudentCode.IsEnabled = false; // Không cho sửa mã học sinh
            txtFormFullName.Text = student.FullName;
            dpFormDateOfBirth.SelectedDate = student.DateOfBirth;
            cmbFormGender.SelectedIndex = (int)student.Gender;
            txtFormClass.Text = student.Class;
            txtFormSchoolYear.Text = student.SchoolYear;
            
            // Set parent
            if (student.ParentId.HasValue && student.ParentId != Guid.Empty)
            {
                cmbFormParent.SelectedValue = student.ParentId;
            }
            else
            {
                cmbFormParent.SelectedIndex = 0; // "Không có phụ huynh"
            }
        }

        private void ClearFormData()
        {
            txtFormStudentCode.Text = GenerateStudentCode();
            txtFormStudentCode.IsEnabled = true;
            txtFormFullName.Text = "";
            dpFormDateOfBirth.SelectedDate = DateTime.Now.AddYears(-6); // Mặc định 6 tuổi
            cmbFormGender.SelectedIndex = -1;
            txtFormClass.Text = "";
            txtFormSchoolYear.Text = DateTime.Now.Year + "-" + (DateTime.Now.Year + 1);
            cmbFormParent.SelectedIndex = 0; // "Không có phụ huynh"
            txtFormNotes.Text = "";
        }

        private string GenerateStudentCode()
        {
            var year = DateTime.Now.Year.ToString();
            var random = new Random();
            var number = random.Next(100, 999);
            return $"STU{year}{number}";
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                txtStatus.Text = _isEditMode ? "Đang cập nhật..." : "Đang thêm mới...";

                // Convert DateOfBirth to UTC for PostgreSQL
                DateTime dateOfBirth = dpFormDateOfBirth.SelectedDate ?? DateTime.Now;
                dateOfBirth = DateTime.SpecifyKind(dateOfBirth, DateTimeKind.Utc);

                var student = new Student
                {
                    StudentCode = txtFormStudentCode.Text.Trim(),
                    FullName = txtFormFullName.Text.Trim(),
                    DateOfBirth = dateOfBirth,
                    Gender = (int)(Gender)cmbFormGender.SelectedIndex,
                    Class = txtFormClass.Text.Trim(),
                    SchoolYear = txtFormSchoolYear.Text.Trim(),
                    CreatedBy = LoginWindow.CurrentUser?.Username ?? "System",
                    UpdatedBy = LoginWindow.CurrentUser?.Username ?? "System"
                };

                // Set parent
                var selectedParent = cmbFormParent.SelectedItem as User;
                if (selectedParent != null && selectedParent.Id != Guid.Empty)
                {
                    student.ParentId = selectedParent.Id;
                }
                else
                {
                    // Set null explicitly for no parent
                    student.ParentId = null;
                }

                if (_isEditMode)
                {
                    student.Id = _selectedStudent!.Id;
                    // Make sure to convert the creation date to UTC
                    student.CreateAt = DateTime.SpecifyKind(_selectedStudent.CreateAt, DateTimeKind.Utc);
                    // Set update time to UTC
                    student.UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    
                    await _studentService.UpdateStudentAsync(student);
                    MessageBox.Show("Cập nhật học sinh thành công!", "Thành công", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Ensure we have a new ID
                    student.Id = Guid.NewGuid();
                    student.CreateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    student.UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    
                    await _studentService.CreateStudentAsync(student);
                    MessageBox.Show("Thêm học sinh thành công!", "Thành công", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                StudentFormOverlay.Visibility = Visibility.Collapsed;
                await LoadStudents();
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nChi tiết: {ex.InnerException.Message}";
                }
                
                MessageBox.Show($"Lỗi: {errorMessage}", "Lỗi", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "Lỗi khi lưu dữ liệu";
            }
        }

        private bool ValidateForm()
        {
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(txtFormStudentCode.Text))
            {
                MessageBox.Show("Vui lòng nhập mã học sinh!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFormStudentCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFormFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFormFullName.Focus();
                return false;
            }

            if (!dpFormDateOfBirth.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng chọn ngày sinh!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                dpFormDateOfBirth.Focus();
                return false;
            }

            if (cmbFormGender.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giới tính!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbFormGender.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFormClass.Text))
            {
                MessageBox.Show("Vui lòng nhập lớp!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFormClass.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFormSchoolYear.Text))
            {
                MessageBox.Show("Vui lòng nhập năm học!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFormSchoolYear.Focus();
                return false;
            }

            // Kiểm tra ngày sinh hợp lệ
            var birthDate = dpFormDateOfBirth.SelectedDate.Value;
            var age = DateTime.Now.Year - birthDate.Year;
            if (birthDate > DateTime.Now.AddYears(-age)) age--;

            if (age < 3 || age > 18)
            {
                MessageBox.Show("Tuổi học sinh phải từ 3 đến 18!", "Validation", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                dpFormDateOfBirth.Focus();
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            StudentFormOverlay.Visibility = Visibility.Collapsed;
        }
    }

    // Model để hiển thị trên DataGrid
    public class StudentDisplayModel
    {
        public Guid Id { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string GenderDisplay { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string SchoolYear { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
        public string ParentName { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}