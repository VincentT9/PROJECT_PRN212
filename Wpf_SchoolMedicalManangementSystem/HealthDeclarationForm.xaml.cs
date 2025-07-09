using BusinessObjects;
using SchoolMedicalManagementSystem.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for HealthDeclarationForm.xaml
    /// </summary>
    public partial class HealthDeclarationForm : Window
    {
        // In a real app, these would be services for interacting with the database
        // private readonly IMedicalDiaryService _medicalDiaryService;

        private List<Student> _students;

        public HealthDeclarationForm()
        {
            InitializeComponent();

            // Initialize services
            // _medicalDiaryService = new MedicalDiaryService();

            // Set default date to today
            dpDeclarationDate.SelectedDate = DateTime.Now;
            dpFromDate.SelectedDate = DateTime.Now.AddDays(-7);

            // Load students
            LoadStudents();

            // Set default declarant
            if (cboDeclarant.Items.Count > 0)
                cboDeclarant.SelectedIndex = 0;

            // Set default attendance status
            if (cboAttendanceStatus.Items.Count > 0)
                cboAttendanceStatus.SelectedIndex = 0;

            // Load history data
            LoadDeclarationHistory();
        }

        private void LoadStudents()
        {
            try
            {
                // In a real application, this would fetch students from the database
                // For now, we'll create some sample data
                _students = new List<Student>
                {
                    new Student {
                        Id = Guid.NewGuid(),
                        FullName = "Nguyễn Văn A",
                        Class = "10A1",
                        DateOfBirth = new DateTime(2005, 1, 15),
                        Gender = Gender.Male
                    },
                    new Student {
                        Id = Guid.NewGuid(),
                        FullName = "Trần Thị B",
                        Class = "10A1",
                        DateOfBirth = new DateTime(2005, 3, 22),
                        Gender = Gender.Female
                    },
                    new Student {
                        Id = Guid.NewGuid(),
                        FullName = "Lê Minh C",
                        Class = "10A2",
                        DateOfBirth = new DateTime(2005, 5, 10),
                        Gender = Gender.Male
                    }
                };

                cboStudent.ItemsSource = _students;
                cboHistoryStudent.ItemsSource = _students;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách học sinh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDeclarationHistory()
        {
            try
            {
                // In a real application, this would fetch declaration history from the database
                // For now, we'll create some sample data
                var declarations = new List<dynamic>
                {
                    new
                    {
                        Id = Guid.NewGuid(),
                        StudentName = "Nguyễn Văn A",
                        DeclarationDate = DateTime.Now.AddDays(-3),
                        DeclarationTime = "08:15",
                        Temperature = "36.8",
                        SymptomsShort = "Không có",
                        AttendanceStatus = "Đi học bình thường"
                    },
                    new
                    {
                        Id = Guid.NewGuid(),
                        StudentName = "Trần Thị B",
                        DeclarationDate = DateTime.Now.AddDays(-2),
                        DeclarationTime = "07:45",
                        Temperature = "37.2",
                        SymptomsShort = "Đau đầu nhẹ",
                        AttendanceStatus = "Đi học nhưng cần theo dõi"
                    },
                    new
                    {
                        Id = Guid.NewGuid(),
                        StudentName = "Lê Minh C",
                        DeclarationDate = DateTime.Now.AddDays(-1),
                        DeclarationTime = "08:00",
                        Temperature = "38.5",
                        SymptomsShort = "Sốt, đau họng",
                        AttendanceStatus = "Nghỉ học (có phép)"
                    }
                };

                dgHealthDeclarationHistory.ItemsSource = declarations;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải lịch sử khai báo: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cboStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedStudent = cboStudent.SelectedItem as Student;
                if (selectedStudent != null)
                {
                    txtClass.Text = selectedStudent.Class;
                    txtDateOfBirth.Text = selectedStudent.DateOfBirth.ToString("dd/MM/yyyy");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy thông tin học sinh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSaveDeclaration_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (cboStudent.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn học sinh", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (dpDeclarationDate.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn ngày khai báo", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cboDeclarant.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn người khai báo", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Collect symptoms
                var symptoms = new List<string>();
                if (chkFever.IsChecked == true) symptoms.Add("Sốt");
                if (chkCough.IsChecked == true) symptoms.Add("Ho");
                if (chkSoreThroat.IsChecked == true) symptoms.Add("Đau họng");
                if (chkRunnyNose.IsChecked == true) symptoms.Add("Sổ mũi");
                if (chkHeadache.IsChecked == true) symptoms.Add("Đau đầu");
                if (chkFatigue.IsChecked == true) symptoms.Add("Mệt mỏi");
                if (chkShortnessOfBreath.IsChecked == true) symptoms.Add("Khó thở");
                if (chkBodyAches.IsChecked == true) symptoms.Add("Đau nhức cơ thể");
                if (chkDiarrhea.IsChecked == true) symptoms.Add("Tiêu chảy");
                if (chkOtherSymptoms.IsChecked == true) symptoms.Add("Khác");

                // Create new medical diary entry
                var healthDeclaration = new HealthDeclaration
                {
                    Id = Guid.NewGuid(),
                    StudentId = ((Student)cboStudent.SelectedItem).Id,
                    DeclarationDate = dpDeclarationDate.SelectedDate.Value,
                    DeclarationTime = txtDeclarationTime.Text,
                    Declarant = ((ComboBoxItem)cboDeclarant.SelectedItem).Content.ToString(),
                    Temperature = txtTemperature.Text,
                    RespiratoryRate = txtRespiratoryRate.Text,
                    HeartRate = txtHeartRate.Text,
                    BloodPressureSystolic = txtSystolic.Text,
                    BloodPressureDiastolic = txtDiastolic.Text,
                    Symptoms = string.Join(", ", symptoms),
                    SymptomDescription = txtSymptomDescription.Text,
                    AttendanceStatus = ((ComboBoxItem)cboAttendanceStatus.SelectedItem).Content.ToString(),
                    CreatedBy = "CurrentUser", // In a real app, this would be the logged-in user
                    UpdatedBy = "CurrentUser",
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };

                // In a real app, we would save to the database
                // _medicalDiaryService.CreateHealthDeclaration(healthDeclaration);

                MessageBox.Show("Đã lưu khai báo sức khỏe thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                // Refresh history data and reset form
                LoadDeclarationHistory();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnResetForm_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            cboStudent.SelectedIndex = -1;
            txtClass.Text = string.Empty;
            txtDateOfBirth.Text = string.Empty;
            dpDeclarationDate.SelectedDate = DateTime.Now;
            txtDeclarationTime.Text = "08:00";
            if (cboDeclarant.Items.Count > 0)
                cboDeclarant.SelectedIndex = 0;

            txtTemperature.Text = string.Empty;
            txtRespiratoryRate.Text = string.Empty;
            txtHeartRate.Text = string.Empty;
            txtSystolic.Text = string.Empty;
            txtDiastolic.Text = string.Empty;

            chkFever.IsChecked = false;
            chkCough.IsChecked = false;
            chkSoreThroat.IsChecked = false;
            chkRunnyNose.IsChecked = false;
            chkHeadache.IsChecked = false;
            chkFatigue.IsChecked = false;
            chkShortnessOfBreath.IsChecked = false;
            chkBodyAches.IsChecked = false;
            chkDiarrhea.IsChecked = false;
            chkOtherSymptoms.IsChecked = false;

            txtSymptomDescription.Text = string.Empty;
            if (cboAttendanceStatus.Items.Count > 0)
                cboAttendanceStatus.SelectedIndex = 0;
        }

        private void btnSearchHistory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // In a real app, this would search the database based on criteria
                // For this demo, we'll just reload the same data
                LoadDeclarationHistory();

                MessageBox.Show("Đã tìm kiếm lịch sử khai báo", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnViewDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                if (button?.DataContext != null)
                {
                    var selectedDeclaration = button.DataContext;
                    var id = selectedDeclaration.GetType().GetProperty("Id").GetValue(selectedDeclaration);
                    var studentName = selectedDeclaration.GetType().GetProperty("StudentName").GetValue(selectedDeclaration) as string;

                    // In a real app, this would fetch the detailed declaration from the database
                    // For now, we'll just show a message
                    MessageBox.Show($"Hiển thị chi tiết khai báo của {studentName}", "Thông tin chi tiết", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xem chi tiết: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                if (button?.DataContext != null)
                {
                    var selectedDeclaration = button.DataContext;
                    var id = selectedDeclaration.GetType().GetProperty("Id").GetValue(selectedDeclaration);
                    var studentName = selectedDeclaration.GetType().GetProperty("StudentName").GetValue(selectedDeclaration) as string;

                    // In a real app, this would generate a printable document
                    // For now, we'll just show a message
                    MessageBox.Show($"In khai báo sức khỏe của {studentName}", "In thông tin", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi in: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Define a placeholder class for HealthDeclaration if not already defined in your project
    // In a real app, this would be in your BusinessObjects project
    public class HealthDeclaration
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public DateTime DeclarationDate { get; set; }
        public string DeclarationTime { get; set; }
        public string Declarant { get; set; }
        public string Temperature { get; set; }
        public string RespiratoryRate { get; set; }
        public string HeartRate { get; set; }
        public string BloodPressureSystolic { get; set; }
        public string BloodPressureDiastolic { get; set; }
        public string Symptoms { get; set; }
        public string SymptomDescription { get; set; }
        public string AttendanceStatus { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}