using BusinessObjects;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class HealthDeclarationForm : Window
    {
        private readonly HealthRecordService _healthRecordService;
        private readonly StudentService _studentService;
        private HealthRecord _currentHealthRecord;
        private bool _isEditMode;

        public HealthDeclarationForm()
        {
            InitializeComponent();

            // Initialize services in constructor
            var healthRecordRepository = new HealthRecordRepository();
            _healthRecordService = new HealthRecordService(healthRecordRepository);

            var studentRepository = new StudentRepository();
            _studentService = new StudentService(studentRepository);

            _currentHealthRecord = new HealthRecord();
            InitializeForm();
            _isEditMode = false;
        }

        public HealthDeclarationForm(HealthRecord healthRecord)
        {
            InitializeComponent();

            // Initialize services in constructor
            var healthRecordRepository = new HealthRecordRepository();
            _healthRecordService = new HealthRecordService(healthRecordRepository);

            var studentRepository = new StudentRepository();
            _studentService = new StudentService(studentRepository);

            InitializeForm();
            _currentHealthRecord = healthRecord;
            _isEditMode = true;
            txtTitle.Text = "CẬP NHẬT SỨC KHỎE HỌC SINH";
            LoadHealthRecordData();
        }

        private async void InitializeForm()
        {
            InitializeComboBoxes();
            await LoadStudents();
        }

        private void InitializeComboBoxes()
        {
            // Initialize Blood Type ComboBox
            var bloodTypes = new List<string>
            {
                "", "A", "B", "AB", "O", "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-"
            };
            cmbBloodType.ItemsSource = bloodTypes;

            // Initialize Vision ComboBoxes
            var visionOptions = new List<string>
            {
                "", "Bình thường", "Cận thị nhẹ", "Cận thị vừa", "Cận thị nặng",
                "Viễn thị", "Loạn thị", "Khác"
            };
            cmbVisionLeft.ItemsSource = visionOptions;
            cmbVisionRight.ItemsSource = visionOptions;

            // Initialize Hearing ComboBoxes
            var hearingOptions = new List<string>
            {
                "", "Bình thường", "Suy giảm nhẹ", "Suy giảm vừa", "Suy giảm nặng", "Điếc"
            };
            cmbHearingLeft.ItemsSource = hearingOptions;
            cmbHearingRight.ItemsSource = hearingOptions;
        }

        private async Task LoadStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                cmbStudent.ItemsSource = students;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách học sinh: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadHealthRecordData()
        {
            if (_currentHealthRecord == null) return;

            cmbStudent.SelectedValue = _currentHealthRecord.StudentId;
            cmbStudent.IsEnabled = false; // Disable student selection in edit mode

            txtHeight.Text = _currentHealthRecord.Height ?? "";
            txtWeight.Text = _currentHealthRecord.Weight ?? "";
            cmbBloodType.SelectedValue = _currentHealthRecord.BloodType ?? "";
            txtAllergies.Text = _currentHealthRecord.Allergies ?? "";
            txtChronicDiseases.Text = _currentHealthRecord.ChronicDiseases ?? "";
            txtPastMedicalHistory.Text = _currentHealthRecord.PastMedicalHistory ?? "";
            cmbVisionLeft.SelectedValue = _currentHealthRecord.VisionLeft ?? "";
            cmbVisionRight.SelectedValue = _currentHealthRecord.VisionRight ?? "";
            cmbHearingLeft.SelectedValue = _currentHealthRecord.HearingLeft ?? "";
            cmbHearingRight.SelectedValue = _currentHealthRecord.HearingRight ?? "";
            txtVaccinationHistory.Text = _currentHealthRecord.VaccinationHistory ?? "";
            txtOtherNotes.Text = _currentHealthRecord.OtherNotes ?? "";

            // Show student info
            if (_currentHealthRecord.Student != null)
            {
                ShowStudentInfo(_currentHealthRecord.Student);
            }
        }

        private void CmbStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedStudent = cmbStudent.SelectedItem as Student;
            if (selectedStudent != null)
            {
                ShowStudentInfo(selectedStudent);
            }
            else
            {
                pnlStudentInfo.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowStudentInfo(Student student)
        {
            txtStudentCode.Text = student.StudentCode ?? "";
            txtStudentClass.Text = student.Class ?? "";
            txtStudentBirth.Text = student.DateOfBirth.ToString("dd/MM/yyyy");
            pnlStudentInfo.Visibility = Visibility.Visible;
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                var healthRecord = CreateHealthRecordFromForm();
                bool success;

                if (_isEditMode)
                {
                    healthRecord.Id = _currentHealthRecord.Id;
                    success = await _healthRecordService.UpdateHealthRecordAsync(healthRecord);
                }
                else
                {
                    success = await _healthRecordService.CreateOrUpdateHealthRecordAsync(healthRecord);
                }

                if (success)
                {
                    MessageBox.Show(
                        _isEditMode ? "Cập nhật hồ sơ sức khỏe thành công!" : "Tạo hồ sơ sức khỏe thành công!",
                        "Thành công",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show(
                        _isEditMode ? "Không thể cập nhật hồ sơ!" : "Không thể tạo hồ sơ!",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateForm()
        {
            if (cmbStudent.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn học sinh!", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbStudent.Focus();
                return false;
            }

            // Validate height if provided
            if (!string.IsNullOrWhiteSpace(txtHeight.Text))
            {
                if (!decimal.TryParse(txtHeight.Text, out var height) || height <= 0 || height > 300)
                {
                    MessageBox.Show("Chiều cao không hợp lệ! (0-300 cm)", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtHeight.Focus();
                    return false;
                }
            }

            // Validate weight if provided
            if (!string.IsNullOrWhiteSpace(txtWeight.Text))
            {
                if (!decimal.TryParse(txtWeight.Text, out var weight) || weight <= 0 || weight > 200)
                {
                    MessageBox.Show("Cân nặng không hợp lệ! (0-200 kg)", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtWeight.Focus();
                    return false;
                }
            }

            return true;
        }

        private HealthRecord CreateHealthRecordFromForm()
        {
            return new HealthRecord
            {
                StudentId = (Guid)cmbStudent.SelectedValue,
                Height = string.IsNullOrWhiteSpace(txtHeight.Text) ? null : txtHeight.Text.Trim(),
                Weight = string.IsNullOrWhiteSpace(txtWeight.Text) ? null : txtWeight.Text.Trim(),
                BloodType = cmbBloodType.SelectedValue?.ToString(),
                Allergies = string.IsNullOrWhiteSpace(txtAllergies.Text) ? null : txtAllergies.Text.Trim(),
                ChronicDiseases = string.IsNullOrWhiteSpace(txtChronicDiseases.Text) ? null : txtChronicDiseases.Text.Trim(),
                PastMedicalHistory = string.IsNullOrWhiteSpace(txtPastMedicalHistory.Text) ? null : txtPastMedicalHistory.Text.Trim(),
                VisionLeft = cmbVisionLeft.SelectedValue?.ToString(),
                VisionRight = cmbVisionRight.SelectedValue?.ToString(),
                HearingLeft = cmbHearingLeft.SelectedValue?.ToString(),
                HearingRight = cmbHearingRight.SelectedValue?.ToString(),
                VaccinationHistory = string.IsNullOrWhiteSpace(txtVaccinationHistory.Text) ? null : txtVaccinationHistory.Text.Trim(),
                OtherNotes = string.IsNullOrWhiteSpace(txtOtherNotes.Text) ? null : txtOtherNotes.Text.Trim(),
                CreatedBy = "Current User", // Replace with actual current user
                UpdatedBy = "Current User"  // Replace with actual current user
            };
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
