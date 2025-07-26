using System;
using System.Windows;
using BusinessObjects;
using DataAccessLayer;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class VaccinationResultForm : Window
    {
        private readonly Student _student;
        private readonly Guid _scheduleId;
        private readonly ScheduleDAO _scheduleDAO = new();

        public VaccinationResultForm(Student student, Guid scheduleId)
        {
            InitializeComponent();
            
            _student = student;
            _scheduleId = scheduleId;
            
            // Set window title and header
            Title = $"Ghi nhận kết quả tiêm phòng - {_student.FullName}";
            txtHeader.Text = $"Ghi nhận kết quả tiêm phòng - {_student.FullName}";
            txtStudentInfo.Text = $"Học sinh: {_student.FullName}";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(txtVaccineDose.Text))
                {
                    MessageBox.Show("Vui lòng nhập liều lượng đã tiêm.", 
                        "Thông tin bắt buộc", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtVaccineDose.Focus();
                    return;
                }
                
                // Format the notes for the VaccinationResult
                string notes = "";
                
                if (!string.IsNullOrWhiteSpace(txtNotes.Text))
                {
                    notes = txtNotes.Text;
                }
                
                // Get the dosage and side effects
                string dosage = txtVaccineDose.Text;
                string sideEffects = txtSideEffects.Text;
                
                // Get current user's full name for tracking who performed the vaccination
                string? updatedBy = LoginWindow.CurrentUser?.FullName;
                
                // Create or update the vaccination result with all details
                _scheduleDAO.UpdateVaccinationResult(_student.Id, _scheduleId, dosage, sideEffects, notes, updatedBy);
                
                MessageBox.Show($"Đã ghi nhận kết quả tiêm chủng cho học sinh {_student.FullName}.",
                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu kết quả tiêm chủng: {ex.Message}", 
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
} 