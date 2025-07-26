using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BusinessObjects;
using DataAccessLayer;
using SchoolMedicalManagementSystem.Enum;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for VaccinationProgramDetails.xaml
    /// </summary>
    public partial class VaccinationProgramDetails : Window
    {
        private readonly CampaignDAO _campaignDAO = new();
        private readonly ScheduleDAO _scheduleDAO = new();
        private Campaign _campaign;
        private bool _isMedicalStaff = false;
        public ObservableCollection<ScheduleWithStudentCount> Schedules { get; set; } = new();

        

        public VaccinationProgramDetails(Campaign campaign, bool isMedicalStaff = false)
        {
            InitializeComponent();
            _campaign = campaign;
            _isMedicalStaff = isMedicalStaff;
            
            // Hide edit and delete buttons for nurses
            if (_isMedicalStaff)
            {
                btnEditProgram.Visibility = Visibility.Collapsed;
                btnDeleteProgram.Visibility = Visibility.Collapsed;
                btnAddSchedule.Visibility = Visibility.Collapsed;
            }
            
            SchedulesDataGrid.ItemsSource = Schedules;
            LoadCampaignDetails();
            LoadSchedules();
            UpdateStatistics();
        }

        private void LoadCampaignDetails()
        {
            HeaderText.Text = $"Chi tiết: {_campaign.Name}";
            ProgramNameText.Text = _campaign.Name ?? "N/A";
            DescriptionText.Text = _campaign.Description ?? "N/A";
            ProgramTypeText.Text = GetTypeDisplay(_campaign.Type);
            StatusText.Text = GetStatusDisplay(_campaign.Status);
            CreateDateText.Text = _campaign.CreateAt.ToString("dd/MM/yyyy HH:mm");
        }

        private string GetTypeDisplay(int? type)
        {
            return type switch
            {
                0 => "Tiêm chủng",
                1 => "Khám sức khỏe",
                _ => "Không xác định"
            };
        }

        private string GetStatusDisplay(int? status)
        {
            return status switch
            {
                0 => "Đã lên kế hoạch",
                1 => "Đang thực hiện",
                2 => "Đã hoàn thành",
                3 => "Đã hủy",
                _ => "Không xác định"
            };
        }

        private void LoadSchedules()
        {
            Schedules.Clear();
            var schedules = _scheduleDAO.GetSchedules()
                .Where(s => s.CampaignId == _campaign.Id)
                .OrderBy(s => s.ScheduledDate)
                .ToList();

            foreach (var schedule in schedules)
            {
                var studentCount = _scheduleDAO.GetStudentIdsByScheduleId(schedule.Id).Count;
                Schedules.Add(new ScheduleWithStudentCount
                {
                    Id = schedule.Id,
                    CampaignId = schedule.CampaignId,
                    ScheduledDate = schedule.ScheduledDate,
                    Location = schedule.Location,
                    Notes = schedule.Notes,
                    CreateAt = schedule.CreateAt,
                    UpdateAt = schedule.UpdateAt,
                    StudentCount = studentCount
                });
            }
        }

        private void UpdateStatistics()
        {
            var totalSchedules = Schedules.Count;
            var completedSchedules = Schedules.Count(s => s.ScheduledDate < DateTime.Today);
            var inProgressSchedules = Schedules.Count(s => s.ScheduledDate.Date == DateTime.Today);
            var upcomingSchedules = Schedules.Count(s => s.ScheduledDate > DateTime.Today);

            TotalSchedulesText.Text = totalSchedules.ToString();
            CompletedSchedulesText.Text = completedSchedules.ToString();
            InProgressSchedulesText.Text = inProgressSchedules.ToString();
            UpcomingSchedulesText.Text = upcomingSchedules.ToString();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EditProgram_Click(object sender, RoutedEventArgs e)
        {
            // Only admins can edit programs
            if (LoginWindow.IsAdmin())
            {
                var form = new VaccinationProgramForm(_campaign);
                if (form.ShowDialog() == true)
                {
                    // Refresh campaign data
                    _campaign = _campaignDAO.GetCampaigns().FirstOrDefault(c => c.Id == _campaign.Id);
                    if (_campaign != null)
                    {
                        LoadCampaignDetails();
                        LoadSchedules();
                        UpdateStatistics();
                    }
                }
            }
            else
            {
                MessageBox.Show("Bạn không có quyền chỉnh sửa chương trình tiêm chủng.",
                    "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteProgram_Click(object sender, RoutedEventArgs e)
        {
            // Only admins can delete programs
            if (!LoginWindow.IsAdmin())
            {
                MessageBox.Show("Bạn không có quyền xóa chương trình tiêm chủng.",
                    "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa chương trình này? Tất cả lịch và dữ liệu liên quan sẽ bị xóa vĩnh viễn.",
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Delete all schedules first
                    var schedules = _scheduleDAO.GetSchedules()
                        .Where(s => s.CampaignId == _campaign.Id)
                        .ToList();

                    foreach (var schedule in schedules)
                    {
                        _scheduleDAO.DeleteSchedule(schedule);
                    }

                    // Delete campaign
                    _campaignDAO.DeleteCampaign(_campaign);

                    MessageBox.Show("Đã xóa chương trình thành công!", "Thông báo", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddSchedule_Click(object sender, RoutedEventArgs e)
        {
            // Only admins can add schedules
            if (!LoginWindow.IsAdmin())
            {
                MessageBox.Show("Bạn không có quyền thêm lịch tiêm chủng mới.",
                    "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            var scheduleForm = new ScheduleForm(_campaign.Id);
            if (scheduleForm.ShowDialog() == true)
            {
                LoadSchedules();
                UpdateStatistics();
            }
        }

        private void ViewScheduleOverview_Click(object sender, RoutedEventArgs e)
        {
            var scheduleView = new VaccinationScheduleView(_isMedicalStaff);
            scheduleView.ShowDialog();
        }

        private void ViewStudents_Click(object sender, RoutedEventArgs e)
        {
            if (SchedulesDataGrid.SelectedItem is ScheduleWithStudentCount selected)
            {
                var studentsView = new ScheduleStudentsView(selected.Id, _isMedicalStaff);
                studentsView.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một lịch để xem danh sách học sinh.", 
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditSchedule_Click(object sender, RoutedEventArgs e)
        {
            // Only admins can edit schedules
            if (!LoginWindow.IsAdmin())
            {
                MessageBox.Show("Bạn không có quyền chỉnh sửa lịch tiêm chủng.",
                    "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if (SchedulesDataGrid.SelectedItem is ScheduleWithStudentCount selected)
            {
                var scheduleForm = new ScheduleForm(_campaign.Id, selected);
                if (scheduleForm.ShowDialog() == true)
                {
                    LoadSchedules();
                    UpdateStatistics();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một lịch để sửa.", 
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteSchedule_Click(object sender, RoutedEventArgs e)
        {
            // Only admins can delete schedules
            if (!LoginWindow.IsAdmin())
            {
                MessageBox.Show("Bạn không có quyền xóa lịch tiêm chủng.",
                    "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if (SchedulesDataGrid.SelectedItem is ScheduleWithStudentCount selected)
            {
                var result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa lịch này?",
                    "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var schedule = new Schedule
                        {
                            Id = selected.Id,
                            CampaignId = selected.CampaignId,
                            ScheduledDate = selected.ScheduledDate,
                            Location = selected.Location,
                            Notes = selected.Notes,
                            CreateAt = selected.CreateAt,
                            UpdateAt = selected.UpdateAt
                        };

                        _scheduleDAO.DeleteSchedule(schedule);
                        LoadSchedules();
                        UpdateStatistics();

                        MessageBox.Show("Đã xóa lịch thành công!", "Thông báo", 
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa lịch: {ex.Message}", "Lỗi", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một lịch để xóa.", 
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

    // Helper class to display student count with schedule
    public class ScheduleWithStudentCount : Schedule
    {
        public int StudentCount { get; set; }
    }
} 