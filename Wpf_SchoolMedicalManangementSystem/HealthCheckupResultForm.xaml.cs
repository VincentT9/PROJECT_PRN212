using System;
using System.Windows;
using System.Text;
using BusinessObjects;
using DataAccessLayer;
using System.Linq; // Added missing import for FirstOrDefault

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class HealthCheckupResultForm : Window
    {
        private readonly Student _student;
        private readonly Guid _scheduleId;
        private readonly ScheduleDAO _scheduleDAO = new();

        public HealthCheckupResultForm(Student student, Guid scheduleId)
        {
            InitializeComponent();
            
            _student = student;
            _scheduleId = scheduleId;
            
            // Set window title and header
            Title = $"Ghi nhận kết quả khám sức khỏe - {_student.FullName}";
            txtHeader.Text = $"Ghi nhận kết quả khám sức khỏe - {_student.FullName}";
            txtStudentInfo.Text = $"Học sinh: {_student.FullName}";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Build the results data
                var healthCheckupResult = new HealthCheckupResult
                {
                    Id = Guid.NewGuid(),
                    ScheduleDetailId = GetScheduleDetailId(),
                    Height = ParseNullableFloat(txtHeight.Text),
                    Weight = ParseNullableFloat(txtWeight.Text),
                    VisionLeftResult = txtVisionLeft.Text,
                    VisionRightResult = txtVisionRight.Text,
                    HearingLeftResult = txtHearingLeft.Text,
                    HearingRightResult = txtHearingRight.Text,
                    BloodPressureSys = ParseNullableFloat(txtSystolicBP.Text),
                    BloodPressureDia = ParseNullableFloat(txtDiastolicBP.Text),
                    HeartRate = ParseNullableFloat(txtHeartRate.Text),
                    DentalCheckupResult = txtDentalResults.Text,
                    OtherResults = txtOtherResults.Text,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };
                
                // Format the notes string
                string notes = FormatHealthCheckupNotes(healthCheckupResult);
                
                // Save the health checkup result
                _scheduleDAO.UpdateStudentHealthCheckupStatus(_student.Id, _scheduleId, notes, healthCheckupResult);
                
                MessageBox.Show($"Đã ghi nhận kết quả khám sức khỏe cho học sinh {_student.FullName}.",
                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu kết quả khám sức khỏe: {ex.Message}", 
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Guid GetScheduleDetailId()
        {
            // Get the schedule detail for this student and schedule
            var scheduleDetails = _scheduleDAO.GetScheduleDetailsByScheduleId(_scheduleId);
            var scheduleDetail = scheduleDetails.FirstOrDefault(sd => sd.StudentId == _student.Id);
            
            if (scheduleDetail != null)
            {
                return scheduleDetail.Id;
            }
            
            // If no schedule detail exists, create one
            var newDetail = new ScheduleDetail
            {
                Id = Guid.NewGuid(),
                StudentId = _student.Id,
                ScheduleId = _scheduleId,
                VaccinationDate = DateTime.Now,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now
            };
            
            _scheduleDAO.AddStudentToSchedule(newDetail);
            return newDetail.Id;
        }
        
        private string FormatHealthCheckupNotes(HealthCheckupResult result)
        {
            var sb = new StringBuilder("Đã khám");
            
            if (result.Height.HasValue)
                sb.Append($" | Chiều cao: {result.Height}cm");
                
            if (result.Weight.HasValue)
                sb.Append($" | Cân nặng: {result.Weight}kg");
                
            if (!string.IsNullOrEmpty(result.VisionLeftResult) || !string.IsNullOrEmpty(result.VisionRightResult))
                sb.Append($" | Thị lực: T{result.VisionLeftResult}/P{result.VisionRightResult}");
                
            if (!string.IsNullOrEmpty(result.HearingLeftResult) || !string.IsNullOrEmpty(result.HearingRightResult))
                sb.Append($" | Thính lực: T{result.HearingLeftResult}/P{result.HearingRightResult}");
                
            if (result.BloodPressureSys.HasValue && result.BloodPressureDia.HasValue)
                sb.Append($" | Huyết áp: {result.BloodPressureSys}/{result.BloodPressureDia}");
                
            if (result.HeartRate.HasValue)
                sb.Append($" | Nhịp tim: {result.HeartRate}");
                
            return sb.ToString();
        }
        
        private float? ParseNullableFloat(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
                
            if (float.TryParse(value, out float result))
                return result;
                
            return null;
        }
        
        private int? ParseNullableInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;
                
            if (int.TryParse(value, out int result))
                return result;
                
            return null;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
} 