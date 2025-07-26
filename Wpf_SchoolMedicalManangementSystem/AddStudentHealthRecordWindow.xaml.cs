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

        public AddStudentHealthRecordWindow()
        {
            InitializeComponent();
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
            // Lấy dữ liệu từ các TextBox (giả sử bạn đã đặt tên cho các TextBox tương ứng)
            var healthRecord = new HealthRecord
            {
                Id = Guid.NewGuid(),
                StudentId = foundStudent.Id,
                BloodType = /* textboxBloodType.Text */ ((TextBox)LogicalTreeHelper.FindLogicalNode(this, "textboxBloodType")).Text,
                Weight = ((TextBox)LogicalTreeHelper.FindLogicalNode(this, "textboxWeight")).Text,
                Height = ((TextBox)LogicalTreeHelper.FindLogicalNode(this, "textboxHeight")).Text,
                Allergies = ((TextBox)LogicalTreeHelper.FindLogicalNode(this, "textboxAllergies")).Text,
                ChronicDiseases = ((TextBox)LogicalTreeHelper.FindLogicalNode(this, "textboxChronicDiseases")).Text,
                VaccinationHistory = ((TextBox)LogicalTreeHelper.FindLogicalNode(this, "textboxVaccinationHistory")).Text,
                VisionLeft = ((TextBox)LogicalTreeHelper.FindLogicalNode(this, "textboxVisionLeft")).Text,
                VisionRight = ((TextBox)LogicalTreeHelper.FindLogicalNode(this, "textboxVisionRight")).Text,
                HearingLeft = ((TextBox)LogicalTreeHelper.FindLogicalNode(this, "textboxHearingLeft")).Text,
                HearingRight = ((TextBox)LogicalTreeHelper.FindLogicalNode(this, "textboxHearingRight")).Text,
                OtherNotes = ((TextBox)LogicalTreeHelper.FindLogicalNode(this, "textboxOtherNotes")).Text,
                CreatedBy = "user",
                UpdatedBy = "user",
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };
            var healthRecordService = new HealthRecordService();
            var result = healthRecordService.CreateHealthRecordAsync(healthRecord);
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