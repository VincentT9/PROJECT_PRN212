using BusinessObjects;
using Repositories;
using Services;
using SchoolMedicalManagementSystem.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class HealthHistoryReportView : Window
    {
        private readonly HealthRecordService _healthRecordService;
        private readonly MedicalIncidentService _medicalIncidentService;
        private readonly StudentService _studentService;

        public HealthHistoryReportView()
        {
            InitializeComponent();

            // Initialize services in constructor
            var healthRecordRepository = new HealthRecordRepository();
            _healthRecordService = new HealthRecordService(healthRecordRepository);

            var medicalIncidentRepository = new MedicalIncidentRepository();
            _medicalIncidentService = new MedicalIncidentService(medicalIncidentRepository);

            var studentRepository = new StudentRepository();
            _studentService = new StudentService(studentRepository);

            InitializeForm();
        }

        private async void InitializeForm()
        {
            InitializeComboBoxes();
            await LoadClasses();

            // Set default date range (current month)
            dpFromDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dpToDate.SelectedDate = DateTime.Now;
        }

        private void InitializeComboBoxes()
        {
            // Initialize Report Type ComboBox
            var reportTypes = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("health_records", "Hồ sơ sức khỏe"),
                new KeyValuePair<string, string>("medical_incidents", "Sự kiện y tế"),
                new KeyValuePair<string, string>("health_summary", "Tổng hợp sức khỏe"),
                new KeyValuePair<string, string>("statistics", "Thống kê")
            };
            cmbReportType.ItemsSource = reportTypes;
            cmbReportType.SelectedIndex = 0;
        }

        private async Task LoadClasses()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                var classes = students.Select(s => s.Class).Distinct().Where(c => !string.IsNullOrEmpty(c)).OrderBy(c => c).ToList();
                classes.Insert(0, "Tất cả");
                cmbClass.ItemsSource = classes;
                cmbClass.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách lớp: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            await GenerateReport();
        }

        private async Task GenerateReport()
        {
            try
            {
                txtStatus.Text = "Đang tạo báo cáo...";

                var selectedReportType = cmbReportType.SelectedValue?.ToString();
                var selectedClass = cmbClass.SelectedValue?.ToString();
                var fromDate = dpFromDate.SelectedDate ?? DateTime.Now.AddMonths(-1);
                var toDate = dpToDate.SelectedDate ?? DateTime.Now;

                switch (selectedReportType)
                {
                    case "health_records":
                        await GenerateHealthRecordsReport(selectedClass);
                        tcReports.SelectedIndex = 0; // Show Health Records tab
                        break;
                    case "medical_incidents":
                        await GenerateMedicalIncidentsReport(selectedClass, fromDate, toDate);
                        tcReports.SelectedIndex = 1; // Show Medical Incidents tab
                        break;
                    case "statistics":
                        await GenerateStatisticsReport(selectedClass, fromDate, toDate);
                        tcReports.SelectedIndex = 2; // Show Statistics tab
                        break;
                    default:
                        await GenerateHealthRecordsReport(selectedClass);
                        tcReports.SelectedIndex = 0;
                        break;
                }

                txtStatus.Text = "Tạo báo cáo thành công";
                txtReportInfo.Text = $"Báo cáo từ {fromDate:dd/MM/yyyy} đến {toDate:dd/MM/yyyy}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo báo cáo: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "Lỗi tạo báo cáo";
            }
        }

        private async Task GenerateHealthRecordsReport(string selectedClass)
        {
            var healthRecords = await _healthRecordService.GetAllHealthRecordsAsync();

            // Filter by class if selected
            if (!string.IsNullOrEmpty(selectedClass) && selectedClass != "Tất cả")
            {
                healthRecords = healthRecords.Where(hr => hr.Student?.Class == selectedClass).ToList();
            }

            // Filter by student search if provided
            if (!string.IsNullOrWhiteSpace(txtStudentSearch.Text))
            {
                var searchTerm = txtStudentSearch.Text.ToLower();
                healthRecords = healthRecords.Where(hr =>
                    hr.Student?.FullName?.ToLower().Contains(searchTerm) == true ||
                    hr.Student?.StudentCode?.ToLower().Contains(searchTerm) == true).ToList();
            }

            // Update summary cards
            var allStudents = await _studentService.GetAllStudentsAsync();
            if (!string.IsNullOrEmpty(selectedClass) && selectedClass != "Tất cả")
            {
                allStudents = allStudents.Where(s => s.Class == selectedClass).ToList();
            }

            txtTotalStudents.Text = allStudents.Count.ToString();
            txtWithHealthRecords.Text = healthRecords.Count.ToString();
            txtWithAllergies.Text = healthRecords.Count(hr => !string.IsNullOrEmpty(hr.Allergies) && hr.Allergies != "Không có").ToString();
            txtWithChronicDiseases.Text = healthRecords.Count(hr => !string.IsNullOrEmpty(hr.ChronicDiseases) && hr.ChronicDiseases != "Không có").ToString();

            // Bind to DataGrid
            dgHealthReports.ItemsSource = healthRecords;
        }

        private async Task GenerateMedicalIncidentsReport(string selectedClass, DateTime fromDate, DateTime toDate)
        {
            var incidents = await _medicalIncidentService.GetMedicalIncidentsByDateRangeAsync(fromDate, toDate);

            // Filter by class if selected
            if (!string.IsNullOrEmpty(selectedClass) && selectedClass != "Tất cả")
            {
                incidents = incidents.Where(inc => inc.Student?.Class == selectedClass).ToList();
            }

            // Filter by student search if provided
            if (!string.IsNullOrWhiteSpace(txtStudentSearch.Text))
            {
                var searchTerm = txtStudentSearch.Text.ToLower();
                incidents = incidents.Where(inc =>
                    inc.Student?.FullName?.ToLower().Contains(searchTerm) == true ||
                    inc.Student?.StudentCode?.ToLower().Contains(searchTerm) == true).ToList();
            }

        
            // Update summary cards
            txtTotalIncidents.Text = incidents.Count.ToString();
            txtAccidents.Text = incidents.Count(inc => inc.IncidentType == (int)IncidentType.Accident).ToString();
            txtFeverCases.Text = incidents.Count(inc => inc.IncidentType == (int)IncidentType.Fever).ToString();
            txtResolvedCases.Text = incidents.Count(inc => inc.Status == (int)IncidentStatus.Resolved).ToString();

            // Bind to DataGrid
            dgIncidentReports.ItemsSource = incidents;
        }

        private async Task GenerateStatisticsReport(string selectedClass, DateTime fromDate, DateTime toDate)
        {
            // Health Statistics
            var healthRecords = await _healthRecordService.GetAllHealthRecordsAsync();
            if (!string.IsNullOrEmpty(selectedClass) && selectedClass != "Tất cả")
            {
                healthRecords = healthRecords.Where(hr => hr.Student?.Class == selectedClass).ToList();
            }

            var healthStats = new List<string>
            {
                $"Tổng số hồ sơ sức khỏe: {healthRecords.Count}",
                $"Học sinh có dị ứng: {healthRecords.Count(hr => !string.IsNullOrEmpty(hr.Allergies) && hr.Allergies != "Không có")}",
                $"Học sinh có bệnh mãn tính: {healthRecords.Count(hr => !string.IsNullOrEmpty(hr.ChronicDiseases) && hr.ChronicDiseases != "Không có")}",
                $"Học sinh có vấn đề thị lực: {healthRecords.Count(hr => (!string.IsNullOrEmpty(hr.VisionLeft) && hr.VisionLeft != "Bình thường") || (!string.IsNullOrEmpty(hr.VisionRight) && hr.VisionRight != "Bình thường"))}",
                "",
                "Phân bố nhóm máu:",
                $"- Nhóm A: {healthRecords.Count(hr => hr.BloodType?.Contains("A") == true)}",
                $"- Nhóm B: {healthRecords.Count(hr => hr.BloodType?.Contains("B") == true && !hr.BloodType.Contains("AB"))}",
                $"- Nhóm AB: {healthRecords.Count(hr => hr.BloodType?.Contains("AB") == true)}",
                $"- Nhóm O: {healthRecords.Count(hr => hr.BloodType?.Contains("O") == true)}",
                $"- Chưa xác định: {healthRecords.Count(hr => string.IsNullOrEmpty(hr.BloodType))}"
            };

            lstHealthStats.ItemsSource = healthStats;

            // Incident Statistics
            var incidents = await _medicalIncidentService.GetMedicalIncidentsByDateRangeAsync(fromDate, toDate);
            if (!string.IsNullOrEmpty(selectedClass) && selectedClass != "Tất cả")
            {
                incidents = incidents.Where(inc => inc.Student?.Class == selectedClass).ToList();
            }

            var incidentStats = new List<string>
            {
                $"Tổng số sự kiện y tế: {incidents.Count}",
                "",
                "Phân loại theo loại sự kiện:",
                $"- Tai nạn: {incidents.Count(inc => inc.IncidentType == (int)IncidentType.Accident)}",
                $"- Sốt: {incidents.Count(inc => inc.IncidentType == (int)IncidentType.Fever)}",
                $"- Ngã: {incidents.Count(inc => inc.IncidentType == (int)IncidentType.Fall)}",
                $"- Dịch bệnh: {incidents.Count(inc => inc.IncidentType == (int)IncidentType.Epidemic)}",
                $"- Khác: {incidents.Count(inc => inc.IncidentType == (int)IncidentType.Other)}",
                "",
                "Phân loại theo trạng thái:",
                $"- Đã báo cáo: {incidents.Count(inc => inc.Status == (int)IncidentStatus.Reported)}",
                $"- Đang xử lý: {incidents.Count(inc => inc.Status == (int)IncidentStatus.Processing)}",
                $"- Đã giải quyết: {incidents.Count(inc => inc.Status == (int)IncidentStatus.Resolved)}"
            };

            lstIncidentStats.ItemsSource = incidentStats;
        }

        private string GetIncidentTypeDisplay(int incidentType)
        {
            return ((IncidentType)incidentType) switch
            {
                IncidentType.Accident => "Tai nạn",
                IncidentType.Fever => "Sốt",
                IncidentType.Fall => "Ngã",
                IncidentType.Epidemic => "Dịch bệnh",
                IncidentType.Other => "Khác",
                _ => "Không xác định"
            };
        }

        private string GetStatusDisplay(int status)
        {
            return ((IncidentStatus)status) switch
            {
                IncidentStatus.Reported => "Đã báo cáo",
                IncidentStatus.Processing => "Đang xử lý",
                IncidentStatus.Resolved => "Đã giải quyết",
                _ => "Không xác định"
            };
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng xuất Excel đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng in báo cáo đang được phát triển!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
