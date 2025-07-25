using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BusinessObjects;
using DataAccessLayer;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for VaccinationScheduleView.xaml
    /// </summary>
    public partial class VaccinationScheduleView : Window
    {
        private readonly ScheduleDAO _scheduleDAO = new();
        private readonly CampaignDAO _campaignDAO = new();
        public ObservableCollection<ScheduleWithCampaignInfo> Schedules { get; set; } = new();
        public ObservableCollection<ScheduleWithCampaignInfo> FilteredSchedules { get; set; } = new();

        public VaccinationScheduleView()
        {
            InitializeComponent();
            SchedulesDataGrid.ItemsSource = FilteredSchedules;
            LoadSchedules();
            UpdateStatistics();
        }

        private void LoadSchedules()
        {
            Schedules.Clear();
            var schedules = _scheduleDAO.GetSchedules();
            var campaigns = _campaignDAO.GetCampaigns();

            foreach (var schedule in schedules)
            {
                var campaign = campaigns.FirstOrDefault(c => c.Id == schedule.CampaignId);
                var studentCount = _scheduleDAO.GetStudentIdsByScheduleId(schedule.Id).Count;

                Schedules.Add(new ScheduleWithCampaignInfo
                {
                    Id = schedule.Id,
                    CampaignId = schedule.CampaignId,
                    CampaignName = campaign?.Name ?? "N/A",
                    Type = campaign?.Type ?? 0,
                    ScheduledDate = schedule.ScheduledDate,
                    Location = schedule.Location,
                    Notes = schedule.Notes,
                    CreateAt = schedule.CreateAt,
                    UpdateAt = schedule.UpdateAt,
                    StudentCount = studentCount
                });
            }
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            FilteredSchedules.Clear();
            var filtered = Schedules.AsEnumerable();

            // Apply search filter
            if (!string.IsNullOrEmpty(SearchBox.Text) && SearchBox.Text != "Tìm kiếm theo địa điểm, ghi chú...")
            {
                filtered = filtered.Where(s => s.Location?.Contains(SearchBox.Text, StringComparison.OrdinalIgnoreCase) == true ||
                                             s.Notes?.Contains(SearchBox.Text, StringComparison.OrdinalIgnoreCase) == true);
            }

            // Apply type filter
            if (TypeFilter != null && TypeFilter.SelectedIndex > 0)
            {
                var selectedType = TypeFilter.SelectedIndex - 1; // 0 = Vaccination, 1 = HealthCheckup
                filtered = filtered.Where(s => s.Type == selectedType);
            }

            // Apply date filter
            if (DateFilter != null && DateFilter.SelectedIndex > 0)
            {
                var today = DateTime.Today;
                filtered = DateFilter.SelectedIndex switch
                {
                    1 => filtered.Where(s => s.ScheduledDate.Date == today), // Today
                    2 => filtered.Where(s => s.ScheduledDate >= today && s.ScheduledDate <= today.AddDays(7)), // This week
                    3 => filtered.Where(s => s.ScheduledDate >= today && s.ScheduledDate <= today.AddMonths(1)), // This month
                    4 => filtered.Where(s => s.ScheduledDate < today), // Past
                    5 => filtered.Where(s => s.ScheduledDate > today), // Future
                    _ => filtered
                };
            }

            // Apply status filter
            if (StatusFilter != null && StatusFilter.SelectedIndex > 0)
            {
                var today = DateTime.Today;
                filtered = StatusFilter.SelectedIndex switch
                {
                    1 => filtered.Where(s => s.ScheduledDate < today), // Completed
                    2 => filtered.Where(s => s.ScheduledDate.Date == today), // In progress
                    3 => filtered.Where(s => s.ScheduledDate > today), // Upcoming
                    _ => filtered
                };
            }

            foreach (var schedule in filtered)
            {
                FilteredSchedules.Add(schedule);
            }
        }

        private void UpdateStatistics()
        {
            var today = DateTime.Today;
            var totalSchedules = Schedules.Count;
            var completedSchedules = Schedules.Count(s => s.ScheduledDate < today);
            var inProgressSchedules = Schedules.Count(s => s.ScheduledDate.Date == today);
            var upcomingSchedules = Schedules.Count(s => s.ScheduledDate > today);
            var totalStudents = Schedules.Sum(s => s.StudentCount);

            TotalSchedulesText.Text = totalSchedules.ToString();
            CompletedSchedulesText.Text = completedSchedules.ToString();
            InProgressSchedulesText.Text = inProgressSchedules.ToString();
            UpcomingSchedulesText.Text = upcomingSchedules.ToString();
            TotalStudentsText.Text = totalStudents.ToString();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadSchedules();
            UpdateStatistics();
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (SchedulesDataGrid.SelectedItem is ScheduleWithCampaignInfo selected)
            {
                var campaign = _campaignDAO.GetCampaigns().FirstOrDefault(c => c.Id == selected.CampaignId);
                if (campaign != null)
                {
                    var detailsView = new VaccinationProgramDetails(campaign);
                    detailsView.ShowDialog();
                    LoadSchedules(); // Refresh after viewing details
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một lịch để xem chi tiết.", 
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ManageStudents_Click(object sender, RoutedEventArgs e)
        {
            if (SchedulesDataGrid.SelectedItem is ScheduleWithCampaignInfo selected)
            {
                var studentsView = new ScheduleStudentsView(selected.Id);
                studentsView.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một lịch để quản lý học sinh.", 
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void Filter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
    }

    // Helper class to display schedule with campaign information
    public class ScheduleWithCampaignInfo : Schedule
    {
        public string CampaignName { get; set; } = "";
        public int Type { get; set; }
        public int StudentCount { get; set; }

        public string TypeDisplay => Type switch
        {
            0 => "Tiêm chủng",
            1 => "Khám sức khỏe",
            _ => "Không xác định"
        };

        public string StatusDisplay
        {
            get
            {
                var today = DateTime.Today;
                return ScheduledDate switch
                {
                    var date when date < today => "Đã hoàn thành",
                    var date when date.Date == today => "Đang thực hiện",
                    var date when date > today => "Sắp tới",
                    _ => "Không xác định"
                };
            }
        }
    }
}
