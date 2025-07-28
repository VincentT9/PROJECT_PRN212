using System;
using System.Collections.ObjectModel;
using System.Windows;
using BusinessObjects;
using DataAccessLayer;
using SchoolMedicalManagementSystem.Enum;
using System.Linq;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for VaccinationProgramForm.xaml
    /// </summary>
    public partial class VaccinationProgramForm : Window
    {
        private readonly CampaignDAO _campaignDAO = new();
        private readonly ScheduleDAO _scheduleDAO = new();
        public ObservableCollection<Schedule> Schedules { get; set; } = new();
        private Campaign? _editingCampaign;

        public VaccinationProgramForm()
        {
            InitializeComponent();
            SchedulesDataGrid.ItemsSource = Schedules;
            ScheduleDatePicker.SelectedDate = DateTime.Today;
        }

        public VaccinationProgramForm(Campaign campaign) : this()
        {
            _editingCampaign = campaign;
            HeaderText.Text = "Chỉnh sửa chương trình";
            LoadCampaignData();
        }

        private void LoadCampaignData()
        {
            if (_editingCampaign == null) return;

            ProgramNameTextBox.Text = _editingCampaign.Name;
            DescriptionTextBox.Text = _editingCampaign.Description;
            
            if (_editingCampaign.Type.HasValue)
            {
                ProgramTypeComboBox.SelectedIndex = _editingCampaign.Type.Value;
            }

            if (_editingCampaign.Status.HasValue)
            {
                StatusComboBox.SelectedIndex = _editingCampaign.Status.Value;
            }

            var existingSchedules = _scheduleDAO.GetSchedules()
                .Where(s => s.CampaignId == _editingCampaign.Id)
                .ToList();

            foreach (var schedule in existingSchedules)
            {
                Schedules.Add(schedule);
            }
        }

        private void AddSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (ScheduleDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(ScheduleLocationTextBox.Text))
            {
                MessageBox.Show("Vui lòng nhập địa điểm.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var schedule = new Schedule
            {
                Id = Guid.NewGuid(),
                ScheduledDate = ScheduleDatePicker.SelectedDate.Value,
                Location = ScheduleLocationTextBox.Text,
                Notes = ScheduleNoteTextBox.Text,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now
            };

            Schedules.Add(schedule);
            ClearScheduleForm();
        }

        private void ClearScheduleForm()
        {
            ScheduleDatePicker.SelectedDate = DateTime.Today;
            ScheduleLocationTextBox.Clear();
            ScheduleNoteTextBox.Clear();
        }

        private void RemoveSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (SchedulesDataGrid.SelectedItem is Schedule selected)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa lịch này?", "Xác nhận", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    Schedules.Remove(selected);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một lịch để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                var campaign = _editingCampaign ?? new Campaign
                {
                    Id = Guid.NewGuid(),
                    CreateAt = DateTime.Now
                };

                campaign.Name = ProgramNameTextBox.Text.Trim();
                campaign.Description = DescriptionTextBox.Text.Trim();
                campaign.Type = ProgramTypeComboBox.SelectedIndex;
                campaign.Status = StatusComboBox.SelectedIndex;
                campaign.UpdateAt = DateTime.Now;

                if (_editingCampaign == null)
                {
                    _campaignDAO.CreateCampaign(campaign);
                    
                    foreach (var schedule in Schedules)
                    {
                       
                        if (schedule.Id == Guid.Empty)
                        {
                            schedule.Id = Guid.NewGuid();
                        }
                        
                        schedule.CampaignId = campaign.Id;
                        
                        // Set create/update dates if not already set
                        if (schedule.CreateAt == default)
                        {
                            schedule.CreateAt = DateTime.Now;
                        }
                        schedule.UpdateAt = DateTime.Now;
                        
                        _scheduleDAO.CreateSchedule(schedule);
                    }
                }
                else
                {
                    _campaignDAO.UpdateCampaign(campaign);
                    
                    try
                    {
                        var existingSchedules = _scheduleDAO.GetSchedules()
                            .Where(s => s.CampaignId == campaign.Id)
                            .ToList();

                        foreach (var existingSchedule in existingSchedules)
                        {
                            _scheduleDAO.DeleteSchedule(existingSchedule);
                        }
                        
                        foreach (var schedule in Schedules)
                        {
                            schedule.Id = Guid.NewGuid();
                            schedule.CampaignId = campaign.Id;
                            
                            schedule.CreateAt = DateTime.Now;
                            schedule.UpdateAt = DateTime.Now;
                            
                            _scheduleDAO.CreateSchedule(schedule);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi cập nhật lịch: {ex.Message}\n" + 
                                      (ex.InnerException != null ? $"Chi tiết: {ex.InnerException.Message}" : ""),
                                      "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                MessageBox.Show(_editingCampaign == null ? "Tạo chương trình thành công!" : "Cập nhật chương trình thành công!", 
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = $"Lỗi khi lưu: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nChi tiết lỗi: {ex.InnerException.Message}";
                }
                
                MessageBox.Show(errorMessage, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(ProgramNameTextBox.Text))
            {
                MessageBox.Show("Tên chương trình không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                ProgramNameTextBox.Focus();
                return false;
            }

            if (ProgramTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn loại chương trình.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                ProgramTypeComboBox.Focus();
                return false;
            }

            if (StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn trạng thái chương trình.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                StatusComboBox.Focus();
                return false;
            }

            if (Schedules.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một lịch tiêm/khám.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                this.Close();
            }
        }
    }
}
