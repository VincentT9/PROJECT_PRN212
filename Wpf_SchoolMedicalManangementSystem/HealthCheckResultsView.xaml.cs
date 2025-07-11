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
        private readonly IStudentService _studentService = new StudentService();
        private readonly IHealthCheckupResultService _healthCheckupResultService = new HealthCheckupResultService();
        public HealthCheckResultsView()
        {
            InitializeComponent();
            LoadStudents();
        }

        private void LoadStudents()
        {
            var students = _studentService.GetAllStudents();
            StudentListBox.ItemsSource = students;
        }

        private void StudentListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StudentListBox.SelectedItem is not Student student) return;

            var results = _healthCheckupResultService.GetByStudentId(student.Id);
            HealthCheckListBox.ItemsSource = results;

            ClearDetails();
        }
        private void HealthCheckListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HealthCheckListBox.SelectedItem is not HealthCheckupResult result) return;

            HeightText.Text = $"{result.Height} cm";
            WeightText.Text = $"{result.Weight} kg";
            VisionText.Text = $"{result.VisionLeftResult} / {result.VisionRightResult}";
            HearingText.Text = $"{result.HearingLeftResult} / {result.HearingRightResult}";
            BloodPressureText.Text = $"{result.BloodPressureSys}/{result.BloodPressureDia}";
            HeartRateText.Text = $"{result.HeartRate} bpm";
            DentalText.Text = result.DentalCheckupResult ?? "";
            AbnormalSignsText.Text = result.AbnormalSigns ?? "";
            RecommendationsText.Text = result.Recommendations ?? "";
            CheckupDateText.Text = result.ScheduleDetail?.VaccinationDate.ToString("dd/MM/yyyy") ?? "(Không rõ)";
            UpdateText.Text = result.UpdateAt.ToString("dd/MM/yyyy");
        }

        private void ClearDetails()
        {
            HeightText.Text = WeightText.Text = VisionText.Text = HearingText.Text = "";
            BloodPressureText.Text = HeartRateText.Text = DentalText.Text = "";
            AbnormalSignsText.Text = RecommendationsText.Text = CheckupDateText.Text = UpdateText.Text = "";
        }
    }
}
