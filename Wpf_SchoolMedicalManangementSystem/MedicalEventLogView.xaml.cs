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
    /// Interaction logic for MedicalEventLogView.xaml
    /// </summary>
    public partial class MedicalEventLogView : Window
    {
        private readonly IMedicalIncidentService _medicalIncidentService;
        private MedicalIncident _currentIncident;
        private List<Student> _students;

        public MedicalEventLogView()
        {
            InitializeComponent();
            _medicalIncidentService = new MedicalIncidentService();

            // Load enum values for comboboxes
            cboIncidentType.ItemsSource = Enum.GetValues(typeof(IncidentType));
            cboIncidentStatus.ItemsSource = Enum.GetValues(typeof(IncidentStatus));

            // Set default date to today
            dpIncidentDate.SelectedDate = DateTime.Now;

            // Load initial data
            LoadStudents();
            LoadMedicalIncidents();
        }

        private void LoadStudents()
        {
            try
            {
                // In a real application, this would fetch students from the database
                // For now, we'll create some sample data
                _students = new List<Student>
                {
                    new Student { Id = Guid.NewGuid(), FullName = "Nguyễn Văn A", Class = "10A1", DateOfBirth = new DateTime(2005, 1, 15) },
                    new Student { Id = Guid.NewGuid(), FullName = "Trần Thị B", Class = "10A1", DateOfBirth = new DateTime(2005, 3, 22) },
                    new Student { Id = Guid.NewGuid(), FullName = "Lê Minh C", Class = "10A2", DateOfBirth = new DateTime(2005, 5, 10) }
                };

                cboStudent.ItemsSource = _students;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách học sinh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadMedicalIncidents()
        {
            try
            {
                // In a real application, this would fetch medical incidents from the database
                // For now, we'll create some sample data
                var incidents = new List<dynamic>
                {
                    new
                    {
                        Id = Guid.NewGuid(),
                        StudentName = "Nguyễn Văn A",
                        IncidentType = IncidentType.Fever,
                        IncidentDate = DateTime.Now.AddDays(-5),
                        Location = "Phòng Y tế",
                        Description = "Sốt nhẹ 37.5 độ",
                        Status = IncidentStatus.Resolved
                    },
                    new
                    {
                        Id = Guid.NewGuid(),
                        StudentName = "Trần Thị B",
                        IncidentType = IncidentType.Accident,
                        IncidentDate = DateTime.Now.AddDays(-2),
                        Location = "Sân trường",
                        Description = "Té ngã khi chơi thể thao",
                        Status = IncidentStatus.Processing
                    }
                };

                dgMedicalIncidents.ItemsSource = incidents;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu sự kiện y tế: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (cboStudent.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn học sinh", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cboIncidentType.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn loại sự kiện", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (dpIncidentDate.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn ngày", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtLocation.Text))
                {
                    MessageBox.Show("Vui lòng nhập địa điểm", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Create or update incident
                if (_currentIncident == null)
                {
                    // Create new
                    _currentIncident = new MedicalIncident
                    {
                        Id = Guid.NewGuid(),
                        StudentId = ((Student)cboStudent.SelectedItem).Id,
                        IncidentType = (IncidentType)cboIncidentType.SelectedItem,
                        IncidentDate = dpIncidentDate.SelectedDate.Value,
                        Description = txtDescription.Text,
                        ActionsTaken = txtActionsTaken.Text,
                        Status = cboIncidentStatus.SelectedItem != null ? (IncidentStatus)cboIncidentStatus.SelectedItem : IncidentStatus.Reported,
                        CreatedBy = "CurrentUser", // In a real app, this would be the logged-in user
                        UpdatedBy = "CurrentUser",
                        CreateAt = DateTime.Now,
                        UpdateAt = DateTime.Now
                    };

                    // In a real app, we would save to the database
                    // _medicalIncidentService.CreateMedicalIncident(_currentIncident);

                    MessageBox.Show("Đã thêm sự kiện y tế mới thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Update existing
                    _currentIncident.StudentId = ((Student)cboStudent.SelectedItem).Id;
                    _currentIncident.IncidentType = (IncidentType)cboIncidentType.SelectedItem;
                    _currentIncident.IncidentDate = dpIncidentDate.SelectedDate.Value;
                    _currentIncident.Description = txtDescription.Text;
                    _currentIncident.ActionsTaken = txtActionsTaken.Text;
                    _currentIncident.Status = cboIncidentStatus.SelectedItem != null ? (IncidentStatus)cboIncidentStatus.SelectedItem : _currentIncident.Status;
                    _currentIncident.UpdatedBy = "CurrentUser";
                    _currentIncident.UpdateAt = DateTime.Now;

                    // In a real app, we would save to the database
                    // _medicalIncidentService.UpdateMedicalIncident(_currentIncident);

                    MessageBox.Show("Đã cập nhật sự kiện y tế thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Refresh data and form
                LoadMedicalIncidents();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the selected incident
                var button = sender as Button;
                if (button?.DataContext != null)
                {
                    _currentIncident = new MedicalIncident(); // In a real app, we would fetch from the database

                    // Get information from selected row
                    var selectedItem = button.DataContext;
                    var id = selectedItem.GetType().GetProperty("Id").GetValue(selectedItem);
                    var studentName = selectedItem.GetType().GetProperty("StudentName").GetValue(selectedItem) as string;
                    var incidentType = (IncidentType)selectedItem.GetType().GetProperty("IncidentType").GetValue(selectedItem);
                    var incidentDate = (DateTime)selectedItem.GetType().GetProperty("IncidentDate").GetValue(selectedItem);
                    var location = selectedItem.GetType().GetProperty("Location").GetValue(selectedItem) as string;
                    var description = selectedItem.GetType().GetProperty("Description").GetValue(selectedItem) as string;
                    var status = (IncidentStatus)selectedItem.GetType().GetProperty("Status").GetValue(selectedItem);

                    // Populate the form
                    var student = _students.FirstOrDefault(s => s.FullName == studentName);
                    if (student != null)
                    {
                        cboStudent.SelectedItem = student;
                    }

                    cboIncidentType.SelectedItem = incidentType;
                    cboIncidentStatus.SelectedItem = status;
                    dpIncidentDate.SelectedDate = incidentDate;
                    txtLocation.Text = location;
                    txtDescription.Text = description;
                    txtActionsTaken.Text = _currentIncident.ActionsTaken; // Assume empty in sample data
                    txtOutcome.Text = ""; // Assume empty in sample data

                    MessageBox.Show("Đã tải thông tin sự kiện để chỉnh sửa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chỉnh sửa: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the selected incident
                var button = sender as Button;
                if (button?.DataContext != null)
                {
                    // Confirm deletion
                    MessageBoxResult result = MessageBox.Show(
                        $"Bạn có chắc chắn muốn xóa sự kiện này không?",
                        "Xác nhận xóa",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Get the ID from selected item
                        var id = button.DataContext.GetType().GetProperty("Id").GetValue(button.DataContext);

                        // In a real app, we would delete from the database
                        // _medicalIncidentService.DeleteMedicalIncident(id);

                        MessageBox.Show("Đã xóa sự kiện y tế thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoadMedicalIncidents();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            cboStudent.SelectedIndex = -1;
            cboIncidentType.SelectedIndex = -1;
            cboIncidentStatus.SelectedIndex = -1;
            dpIncidentDate.SelectedDate = DateTime.Now;
            txtLocation.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtActionsTaken.Text = string.Empty;
            txtOutcome.Text = string.Empty;
            _currentIncident = null;
        }
    }
}