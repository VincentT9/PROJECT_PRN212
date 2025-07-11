using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BusinessObjects;
using Services;
using Services.Interface;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for HealthCheckResultsView.xaml
    /// </summary>
    public partial class HealthCheckResultsView : Window
    {
        private readonly IScheduleService _scheduleService = new ScheduleService();
        private readonly IHealthCheckupResultService _healthService = new HealthCheckupResultService();
        private readonly IScheduleDetailService _scheduleDetailService = new ScheduleDetailService();

        private Schedule? selectedSchedule;
        private ScheduleDetail? selectedScheduleDetail;

        public HealthCheckResultsView()
        {
            InitializeComponent();
            LoadSchedules();
        }

        private void LoadSchedules()
        {
            var schedules = _scheduleService.GetActiveSchedules(); // lọc trạng thái đang diễn ra
            ScheduleComboBox.ItemsSource = schedules;
        }

        private void ScheduleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSchedule = ScheduleComboBox.SelectedItem as Schedule;
            if (selectedSchedule == null) return;

            var details = _scheduleDetailService.GetByScheduleId(selectedSchedule.Id);
            StudentListView.ItemsSource = details.Select(d => d.Student).ToList();
        }

        private void StudentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var student = StudentListView.SelectedItem as Student;
            if (student == null || selectedSchedule == null) return;

            selectedScheduleDetail = _scheduleDetailService.GetByStudentAndSchedule(student.Id, selectedSchedule.Id);

            if (selectedScheduleDetail == null) return;

            var result = _healthService.GetByScheduleDetailId(selectedScheduleDetail.Id);

            if (result != null)
            {
                HeightTextBox.Text = result.Height?.ToString();
                WeightTextBox.Text = result.Weight?.ToString();
                VisionLeftTextBox.Text = result.VisionLeftResult;
                VisionRightTextBox.Text = result.VisionRightResult;
                HearingLeftTextBox.Text = result.HearingLeftResult;
                HearingRightTextBox.Text = result.HearingRightResult;
                BloodPressureSysTextBox.Text = result.BloodPressureSys?.ToString();
                BloodPressureDiaTextBox.Text = result.BloodPressureDia?.ToString();
                HeartRateTextBox.Text = result.HeartRate?.ToString();
                DentalCheckupResultTextBox.Text = result.DentalCheckupResult;
                OtherResultsTextBox.Text = result.OtherResults;
                AbnormalSignsTextBox.Text = result.AbnormalSigns;
                RecommendationsTextBox.Text = result.Recommendations;
            }
            else
            {
                HeightTextBox.Clear();
                WeightTextBox.Clear();
                VisionLeftTextBox.Clear();
                VisionRightTextBox.Clear();
                HearingLeftTextBox.Clear();
                HearingRightTextBox.Clear();
                BloodPressureSysTextBox.Clear();
                BloodPressureDiaTextBox.Clear();
                HeartRateTextBox.Clear();
                DentalCheckupResultTextBox.Clear();
                OtherResultsTextBox.Clear();
                AbnormalSignsTextBox.Clear();
                RecommendationsTextBox.Clear();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedScheduleDetail == null)
            {
                MessageBox.Show("Vui lòng chọn học sinh.");
                return;
            }

            var result = _healthService.GetByScheduleDetailId(selectedScheduleDetail.Id);
            if (result == null)
            {
                result = new HealthCheckupResult
                {
                    Id = Guid.NewGuid(),
                    ScheduleDetailId = selectedScheduleDetail.Id,
                    CreateAt = DateTime.Now
                };
                _healthService.Add(result);
            }

            result.Height = float.TryParse(HeightTextBox.Text, out var h) ? h : null;
            result.Weight = float.TryParse(WeightTextBox.Text, out var w) ? w : null;
            result.VisionLeftResult = VisionLeftTextBox.Text;
            result.VisionRightResult = VisionRightTextBox.Text;
            result.AbnormalSigns = AbnormalSignsTextBox.Text;
            result.Recommendations = RecommendationsTextBox.Text;
            result.UpdateAt = DateTime.Now;
            result.HearingLeftResult = HearingLeftTextBox.Text;
            result.HearingRightResult = HearingRightTextBox.Text;
            result.BloodPressureSys = float.TryParse(BloodPressureSysTextBox.Text, out var sys) ? sys : null;
            result.BloodPressureDia = float.TryParse(BloodPressureDiaTextBox.Text, out var dia) ? dia : null;
            result.HeartRate = float.TryParse(HeartRateTextBox.Text, out var hr) ? hr : null;
            result.DentalCheckupResult = DentalCheckupResultTextBox.Text;
            result.OtherResults = OtherResultsTextBox.Text;

            _healthService.Update(result);

            MessageBox.Show("Đã ghi nhận kết quả.");
        }
    }
}