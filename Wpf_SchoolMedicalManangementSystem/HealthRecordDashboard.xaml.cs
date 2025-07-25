using BusinessObjects;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class HealthRecordDashboard : Page, INotifyPropertyChanged
    {
        private readonly HealthRecordService _healthRecordService;
        private readonly StudentService _studentService;
        private ObservableCollection<HealthRecord> _healthRecords;
        private string _searchTerm;
        private string _selectedClass;
        private string _selectedHealthStatus;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<HealthRecord> HealthRecords
        {
            get => _healthRecords;
            set
            {
                _healthRecords = value;
                OnPropertyChanged(nameof(HealthRecords));
                UpdateRecordCount();
            }
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));
            }
        }

        public string SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
            }
        }

        public string SelectedHealthStatus
        {
            get => _selectedHealthStatus;
            set
            {
                _selectedHealthStatus = value;
                OnPropertyChanged(nameof(SelectedHealthStatus));
            }
        }

        public HealthRecordDashboard()
        {
            InitializeComponent();
            DataContext = this;

            var healthRecordRepository = new HealthRecordRepository();
            _healthRecordService = new HealthRecordService(healthRecordRepository);

            var studentRepository = new StudentRepository();
            _studentService = new StudentService(studentRepository);

            HealthRecords = new ObservableCollection<HealthRecord>();

            InitializeComboBoxes();
            LoadHealthRecords();
        }

        private async void InitializeComboBoxes()
        {
            try
            {
                // Load classes
                var students = await _studentService.GetAllStudentsAsync();
                var classes = students.Select(s => s.Class).Distinct().Where(c => !string.IsNullOrEmpty(c)).OrderBy(c => c).ToList();
                classes.Insert(0, "Tất cả");
                cmbClass.ItemsSource = classes;
                cmbClass.SelectedIndex = 0;

                // Initialize Health Status ComboBox
                var healthStatuses = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("", "Tất cả"),
                    new KeyValuePair<string, string>("normal", "Bình thường"),
                    new KeyValuePair<string, string>("allergies", "Có dị ứng"),
                    new KeyValuePair<string, string>("chronic", "Bệnh mãn tính"),
                    new KeyValuePair<string, string>("vision_problems", "Vấn đề thị lực")
                };
                cmbHealthStatus.ItemsSource = healthStatuses;
                cmbHealthStatus.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi khởi tạo dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoadHealthRecords()
        {
            try
            {
                txtStatus.Text = "Đang tải dữ liệu...";

                var records = await _healthRecordService.GetAllHealthRecordsAsync();

                HealthRecords.Clear();
                foreach (var record in records)
                {
                    HealthRecords.Add(record);
                }

                txtStatus.Text = "Tải dữ liệu thành công";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "Lỗi tải dữ liệu";
            }
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            await PerformSearch();
        }

        private async void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            SearchTerm = string.Empty;
            SelectedClass = string.Empty;
            SelectedHealthStatus = string.Empty;

            cmbClass.SelectedIndex = 0;
            cmbHealthStatus.SelectedIndex = 0;

            LoadHealthRecords();
            ClearDetailsPanel();
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addForm = new HealthDeclarationForm();
            if (addForm.ShowDialog() == true)
            {
                LoadHealthRecords();
            }
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var healthRecord = button?.Tag as HealthRecord;

            if (healthRecord != null)
            {
                var editForm = new HealthDeclarationForm(healthRecord);
                if (editForm.ShowDialog() == true)
                {
                    LoadHealthRecords();
                    LoadHealthRecordDetails(healthRecord);
                }
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var healthRecord = button?.Tag as HealthRecord;

            if (healthRecord != null)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa hồ sơ sức khỏe của học sinh:\n{healthRecord.Student?.FullName}?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var success = await _healthRecordService.DeleteHealthRecordAsync(healthRecord.Id);
                        if (success)
                        {
                            MessageBox.Show("Xóa hồ sơ thành công!", "Thành công",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadHealthRecords();
                            ClearDetailsPanel();
                        }
                        else
                        {
                            MessageBox.Show("Không thể xóa hồ sơ!", "Lỗi",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void DgHealthRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRecord = dgHealthRecords.SelectedItem as HealthRecord;
            if (selectedRecord != null)
            {
                LoadHealthRecordDetails(selectedRecord);
            }
        }

        private void LoadHealthRecordDetails(HealthRecord record)
        {
            if (record?.Student == null) return;

            // Student Info
            txtDetailName.Text = record.Student.FullName ?? "";
            txtDetailClass.Text = record.Student.Class ?? "";
            txtDetailBirth.Text = record.Student.DateOfBirth.ToString("dd/MM/yyyy");

            // Physical Info
            txtDetailHeight.Text = $"{record.Height ?? "Chưa có"} cm";
            txtDetailWeight.Text = $"{record.Weight ?? "Chưa có"} kg";
            txtDetailBloodType.Text = record.BloodType ?? "Chưa xác định";

            // Health Conditions
            txtDetailAllergies.Text = record.Allergies ?? "Không có";
            txtDetailChronicDiseases.Text = record.ChronicDiseases ?? "Không có";
            txtDetailMedicalHistory.Text = record.PastMedicalHistory ?? "Không có";

            // Vision and Hearing
            txtDetailVisionLeft.Text = record.VisionLeft ?? "Chưa kiểm tra";
            txtDetailVisionRight.Text = record.VisionRight ?? "Chưa kiểm tra";

            // Vaccination and Notes
            txtDetailVaccination.Text = record.VaccinationHistory ?? "Chưa có thông tin";
            txtDetailOtherNotes.Text = record.OtherNotes ?? "Không có";
        }

        private void ClearDetailsPanel()
        {
            txtDetailName.Text = "";
            txtDetailClass.Text = "";
            txtDetailBirth.Text = "";
            txtDetailHeight.Text = "";
            txtDetailWeight.Text = "";
            txtDetailBloodType.Text = "";
            txtDetailAllergies.Text = "";
            txtDetailChronicDiseases.Text = "";
            txtDetailMedicalHistory.Text = "";
            txtDetailVisionLeft.Text = "";
            txtDetailVisionRight.Text = "";
            txtDetailVaccination.Text = "";
            txtDetailOtherNotes.Text = "";
        }

        private async Task PerformSearch()
        {
            try
            {
                txtStatus.Text = "Đang tìm kiếm...";

                List<HealthRecord> results;

                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    results = await _healthRecordService.SearchHealthRecordsAsync(SearchTerm);
                }
                else
                {
                    results = await _healthRecordService.GetAllHealthRecordsAsync();
                }

                // Apply class filter
                if (!string.IsNullOrEmpty(SelectedClass) && SelectedClass != "Tất cả")
                {
                    results = results.Where(x => x.Student?.Class == SelectedClass).ToList();
                }

                // Apply health status filter
                if (!string.IsNullOrEmpty(SelectedHealthStatus))
                {
                    results = FilterByHealthStatus(results, SelectedHealthStatus);
                }

                HealthRecords.Clear();
                foreach (var record in results)
                {
                    HealthRecords.Add(record);
                }

                txtStatus.Text = $"Tìm thấy {results.Count} kết quả";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                txtStatus.Text = "Lỗi tìm kiếm";
            }
        }

        private List<HealthRecord> FilterByHealthStatus(List<HealthRecord> records, string healthStatus)
        {
            return healthStatus switch
            {
                "allergies" => records.Where(r => !string.IsNullOrEmpty(r.Allergies) && r.Allergies != "Không có").ToList(),
                "chronic" => records.Where(r => !string.IsNullOrEmpty(r.ChronicDiseases) && r.ChronicDiseases != "Không có").ToList(),
                "vision_problems" => records.Where(r =>
                    (!string.IsNullOrEmpty(r.VisionLeft) && r.VisionLeft != "Bình thường") ||
                    (!string.IsNullOrEmpty(r.VisionRight) && r.VisionRight != "Bình thường")).ToList(),
                "normal" => records.Where(r =>
                    (string.IsNullOrEmpty(r.Allergies) || r.Allergies == "Không có") &&
                    (string.IsNullOrEmpty(r.ChronicDiseases) || r.ChronicDiseases == "Không có")).ToList(),
                _ => records
            };
        }

        private void UpdateRecordCount()
        {
            txtRecordCount.Text = $"Tổng số: {HealthRecords?.Count ?? 0} bản ghi";
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
