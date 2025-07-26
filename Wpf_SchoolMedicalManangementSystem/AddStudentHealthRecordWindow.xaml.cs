using System.Windows;
using System.Windows.Controls;
using DataAccessLayer;
using BusinessObjects;
using Services;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class AddStudentHealthRecordWindow : UserControl
    {
        private readonly StudentService studentService = new StudentService();
        private Student? foundStudent;
        User? currentUser;
        public AddStudentHealthRecordWindow()
        {
            InitializeComponent();
            currentUser = App.Current.Properties["CurrentUser"] as BusinessObjects.User;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var parent = Window.GetWindow(this) as ParentDashboar;
            if (parent != null)
            {
                parent.MainContent.Content = new HealthRecordWindow();
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string studentCode = txtStudentCode.Text.Trim();
            Student student = studentService.GetStudentByStudentCodeAsync(studentCode).Result;
            if (student != null)
            {
                foundStudent = student;
                studentInfoPanel.Visibility = Visibility.Visible;
                txtStudentName.Text = $"Họ và tên: {student.FullName}";
                txtStudentClass.Text = $"Lớp: {student.Class}";
                txtStudentYear.Text = $"Năm học: {student.SchoolYear}";
                txtStudentDOB.Text = $"Ngày sinh: {student.DateOfBirth:dd/MM/yyyy}";
                // Enable health record input controls
            }
            else
            {
                foundStudent = null;
                studentInfoPanel.Visibility = Visibility.Collapsed;
                MessageBox.Show("Không tìm thấy học sinh.");
            }
        }

        private void Complete_Click(object sender, RoutedEventArgs e)
        {
            if (foundStudent == null)
            {
                MessageBox.Show("Vui lòng tìm học sinh trước!");
                return;
            }
            
            var healthRecord = new HealthRecord
            {
                Id = Guid.NewGuid(),
                StudentId = foundStudent.Id,
                BloodType = textboxBloodType.Text,
                Weight = textboxWeight.Text,
                Height = textboxHeight.Text,
                Allergies = textboxAllergies.Text,
                ChronicDiseases = textboxChronicDiseases.Text,
                VaccinationHistory = textboxVaccinationHistory.Text,
                VisionLeft = textboxVisionLeft.Text,
                VisionRight = textboxVisionRight.Text,
                HearingLeft = textboxHearingLeft.Text,
                HearingRight = textboxHearingRight.Text,
                OtherNotes = textboxOtherNotes.Text,
                CreatedBy = currentUser.Username,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };
            var healthRecordService = new HealthRecordService();
            var result = healthRecordService.CreateHealthRecordAsync(healthRecord);
            foundStudent.ParentId = currentUser.Id;
            studentService.UpdateStudentAsync(foundStudent);
            if (result)
            {
                MessageBox.Show("Tạo hồ sơ sức khỏe thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                var parent = Window.GetWindow(this) as ParentDashboar;
                if (parent != null)
                {
                    parent.MainContent.Content = new HealthRecordWindow();
                }
            }
            else
            {
                MessageBox.Show("Tạo hồ sơ sức khỏe thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}