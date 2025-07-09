using BusinessObjects;
using SchoolMedicalManagementSystem.Enum;
using Services;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for HealthRecordDashboard.xaml
    /// </summary>
    public partial class HealthRecordDashboard : Window
    {
        private readonly IHealthRecordService _healthRecordService;
        private Student _selectedStudent;
        private HealthRecord _currentRecord;
        private List<Student> _students;

        public HealthRecordDashboard()
        {
            InitializeComponent();
            _healthRecordService = new HealthRecordService();
            LoadStudents();
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

                lstStudents.ItemsSource = _students;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách học sinh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lstStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedStudent = lstStudents.SelectedItem as Student;

                if (_selectedStudent != null)
                {
                    txtStudentName.Text = _selectedStudent.FullName;
                    txtClass.Text = _selectedStudent.Class;
                    txtDOB.Text = _selectedStudent.DateOfBirth.ToString("dd/MM/yyyy");

                    // Load health record
                    LoadHealthRecord();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin học sinh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadHealthRecord()
        {
            try
            {
                // In a real application, this would fetch the health record from the database
                // For now, we'll create a sample health record
                _currentRecord = new HealthRecord
                {
                    Id = Guid.NewGuid(),
                    StudentId = _selectedStudent.Id,
                    Height = "165",
                    Weight = "55",
                    BloodType = "A",
                    VisionLeft = "0.8",
                    VisionRight = "0.9",
                    HearingLeft = "Normal",
                    HearingRight = "Normal",
                    ChronicDiseases = "Không",
                    Allergies = "Dị ứng phấn hoa",
                    PastMedicalHistory = "Từng bị sốt xuất huyết năm 2022",
                    VaccinationHistory = "Đã tiêm đầy đủ các mũi vaccine cơ bản",
                    OtherNotes = "",
                    CreatedBy = "Admin",
                    UpdatedBy = "Admin",
                    CreateAt = DateTime.Now.AddMonths(-6),
                    UpdateAt = DateTime.Now.AddDays(-30)
                };

                // Populate the form
                txtHeight.Text = _currentRecord.Height;
                txtWeight.Text = _currentRecord.Weight;

                // Set blood type in combobox
                if (!string.IsNullOrEmpty(_currentRecord.BloodType))
                {
                    foreach (ComboBoxItem item in cboBloodType.Items)
                    {
                        if (item.Content.ToString() == _currentRecord.BloodType)
                        {
                            cboBloodType.SelectedItem = item;
                            break;
                        }
                    }
                }

                txtVisionLeft.Text = _currentRecord.VisionLeft;
                txtVisionRight.Text = _currentRecord.VisionRight;
                txtHearingLeft.Text = _currentRecord.HearingLeft;
                txtHearingRight.Text = _currentRecord.HearingRight;
                txtChronicDiseases.Text = _currentRecord.ChronicDiseases;
                txtAllergies.Text = _currentRecord.Allergies;
                txtPastMedicalHistory.Text = _currentRecord.PastMedicalHistory;
                txtVaccinationHistory.Text = _currentRecord.VaccinationHistory;
                txtOtherNotes.Text = _currentRecord.OtherNotes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải hồ sơ sức khỏe: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text.ToLower();

                if (string.IsNullOrEmpty(searchText))
                {
                    LoadStudents();
                    return;
                }

                // Filter students based on search text
                var filteredStudents = _students.Where(s =>
                    s.FullName.ToLower().Contains(searchText) ||
                    s.Class.ToLower().Contains(searchText)
                ).ToList();

                lstStudents.ItemsSource = filteredStudents;

                if (filteredStudents.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy học sinh nào phù hợp", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng thêm mới học sinh sẽ được phát triển sau", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnUpdateBasicInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentRecord == null)
                {
                    MessageBox.Show("Vui lòng chọn học sinh trước", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Update health record with form data
                _currentRecord.Height = txtHeight.Text;
                _currentRecord.Weight = txtWeight.Text;
                _currentRecord.BloodType = cboBloodType.SelectedItem != null ? ((ComboBoxItem)cboBloodType.SelectedItem).Content.ToString() : null;
                _currentRecord.VisionLeft = txtVisionLeft.Text;
                _currentRecord.VisionRight = txtVisionRight.Text;
                _currentRecord.HearingLeft = txtHearingLeft.Text;
                _currentRecord.HearingRight = txtHearingRight.Text;
                _currentRecord.ChronicDiseases = txtChronicDiseases.Text;
                _currentRecord.Allergies = txtAllergies.Text;
                _currentRecord.UpdatedBy = "CurrentUser"; // In a real app, this would be the logged-in user
                _currentRecord.UpdateAt = DateTime.Now;

                // In a real app, we would save to the database
                // _healthRecordService.UpdateHealthRecord(_currentRecord);

                MessageBox.Show("Đã cập nhật thông tin cơ bản thành công", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdateMedicalHistory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentRecord == null)
                {
                    MessageBox.Show("Vui lòng chọn học sinh trước", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Update medical history
                _currentRecord.PastMedicalHistory = txtPastMedicalHistory.Text;
                _currentRecord.UpdatedBy = "CurrentUser"; // In a real app, this would be the logged-in user
                _currentRecord.UpdateAt = DateTime.Now;

                // In a real app, we would save to the database
                // _healthRecordService.UpdateHealthRecord(_currentRecord);

                MessageBox.Show("Đã cập nhật tiền sử bệnh thành công", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdateVaccination_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentRecord == null)
                {
                    MessageBox.Show("Vui lòng chọn học sinh trước", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Update vaccination history
                _currentRecord.VaccinationHistory = txtVaccinationHistory.Text;
                _currentRecord.UpdatedBy = "CurrentUser"; // In a real app, this would be the logged-in user
                _currentRecord.UpdateAt = DateTime.Now;

                // In a real app, we would save to the database
                // _healthRecordService.UpdateHealthRecord(_currentRecord);

                MessageBox.Show("Đã cập nhật lịch sử tiêm chủng thành công", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdateOtherNotes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentRecord == null)
                {
                    MessageBox.Show("Vui lòng chọn học sinh trước", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Update other notes
                _currentRecord.OtherNotes = txtOtherNotes.Text;
                _currentRecord.UpdatedBy = "CurrentUser"; // In a real app, this would be the logged-in user
                _currentRecord.UpdateAt = DateTime.Now;

                // In a real app, we would save to the database
                // _healthRecordService.UpdateHealthRecord(_currentRecord);

                MessageBox.Show("Đã cập nhật ghi chú khác thành công", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}