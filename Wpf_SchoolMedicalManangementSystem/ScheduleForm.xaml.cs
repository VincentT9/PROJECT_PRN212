using System;
using System.Windows;
using BusinessObjects;
using DataAccessLayer;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for ScheduleForm.xaml
    /// </summary>
    public partial class ScheduleForm : Window
    {
        private readonly ScheduleDAO _scheduleDAO = new();
        private Guid _campaignId;
        private Schedule? _editingSchedule;

        public ScheduleForm(Guid campaignId)
        {
            InitializeComponent();
            _campaignId = campaignId;
            ScheduleDatePicker.SelectedDate = DateTime.Today;
        }

        public ScheduleForm(Guid campaignId, ScheduleWithStudentCount schedule) : this(campaignId)
        {
            _editingSchedule = schedule;
            HeaderText.Text = "Sửa lịch";
            LoadScheduleData();
        }

        private void LoadScheduleData()
        {
            if (_editingSchedule == null) return;

            ScheduleDatePicker.SelectedDate = _editingSchedule.ScheduledDate;
            LocationTextBox.Text = _editingSchedule.Location;
            NotesTextBox.Text = _editingSchedule.Notes;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                var schedule = _editingSchedule ?? new Schedule
                {
                    Id = Guid.NewGuid(),
                    CampaignId = _campaignId,
                    CreateAt = DateTime.Now
                };

                schedule.ScheduledDate = ScheduleDatePicker.SelectedDate.Value;
                schedule.Location = LocationTextBox.Text.Trim();
                schedule.Notes = NotesTextBox.Text.Trim();
                schedule.UpdateAt = DateTime.Now;

                if (_editingSchedule == null)
                {
                    // Create new schedule
                    _scheduleDAO.CreateSchedule(schedule);
                }
                else
                {
                    // Update existing schedule
                    _scheduleDAO.UpdateSchedule(schedule);
                }

                MessageBox.Show(_editingSchedule == null ? "Tạo lịch thành công!" : "Cập nhật lịch thành công!", 
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateForm()
        {
            if (ScheduleDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                ScheduleDatePicker.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(LocationTextBox.Text))
            {
                MessageBox.Show("Vui lòng nhập địa điểm.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                LocationTextBox.Focus();
                return false;
            }

            return true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn hủy? Tất cả thay đổi sẽ bị mất.", 
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                this.DialogResult = false;
                this.Close();
            }
        }
    }
} 