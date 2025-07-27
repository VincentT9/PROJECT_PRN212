using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using BusinessObjects;
using DataAccessLayer;
using SchoolMedicalManagementSystem.Enum;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for VaccinationProgramView.xaml
    /// </summary>
    public partial class VaccinationProgramView : Page
    {
        private readonly CampaignDAO _campaignDAO = new();
        public ObservableCollection<Campaign> Programs { get; set; } = new();
        public ObservableCollection<Campaign> FilteredPrograms { get; set; } = new();
        private bool _isMedicalStaff = false;

        public VaccinationProgramView()
        {
            InitializeComponent(); // Đảm bảo luôn gọi đầu tiên
            
            // Check if current user is a nurse
            _isMedicalStaff = LoginWindow.IsMedicalStaff();
            
            // Hide Create button if user is a nurse
            if (_isMedicalStaff)
            {
                btnCreateProgram.Visibility = Visibility.Collapsed;
            }
            
            ProgramsDataGrid.ItemsSource = FilteredPrograms;
            LoadPrograms();
        }

        private void LoadPrograms()
        {
            Programs.Clear();
            foreach (var c in _campaignDAO.GetCampaigns())
            {
                Programs.Add(c);
            }
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            FilteredPrograms.Clear();
            var filtered = Programs.AsEnumerable();

            // Apply search filter
            if (!string.IsNullOrEmpty(SearchBox.Text) && SearchBox.Text != "Tìm kiếm theo tên chương trình...")
            {
                filtered = filtered.Where(p => p.Name?.Contains(SearchBox.Text, StringComparison.OrdinalIgnoreCase) == true);
            }

            // Apply type filter
            if (TypeFilter != null && TypeFilter.SelectedIndex > 0)
            {
                var selectedType = TypeFilter.SelectedIndex - 1; // 0 = Vaccination, 1 = HealthCheckup
                filtered = filtered.Where(p => p.Type == selectedType);
            }

            // Apply status filter
            if (StatusFilter != null && StatusFilter.SelectedIndex > 0)
            {
                var selectedStatus = StatusFilter.SelectedIndex - 1; // 0 = Planned, 1 = InProgress, 2 = Completed, 3 = Cancelled
                filtered = filtered.Where(p => p.Status == selectedStatus);
            }

            foreach (var program in filtered)
            {
                FilteredPrograms.Add(program);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadPrograms();
        }

        private void CreateProgram_Click(object sender, RoutedEventArgs e)
        {
            // Only admins can create programs
            if (LoginWindow.IsAdmin())
            {
                var form = new VaccinationProgramForm();
                if (form.ShowDialog() == true)
                {
                    LoadPrograms();
                }
            }
            else
            {
                MessageBox.Show("Bạn không có quyền thêm chương trình tiêm chủng mới.", 
                    "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void EditProgram_Click(object sender, RoutedEventArgs e)
        {
            // Only admins can edit programs
            if (!LoginWindow.IsAdmin())
            {
                MessageBox.Show("Bạn không có quyền chỉnh sửa chương trình tiêm chủng.", 
                    "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if (ProgramsDataGrid.SelectedItem is Campaign selected)
            {
                var form = new VaccinationProgramForm(selected);
                if (form.ShowDialog() == true)
                {
                    LoadPrograms();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một chương trình để sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (ProgramsDataGrid.SelectedItem is Campaign selected)
            {
                var detailsView = new VaccinationProgramDetails(selected, _isMedicalStaff);
                detailsView.ShowDialog();
                LoadPrograms(); // Refresh after viewing details
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một chương trình để xem chi tiết.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
    }

    // Extension class to add display properties to Campaign
    public static class CampaignExtensions
    {
        public static string TypeDisplay(this Campaign campaign)
        {
            return campaign.Type switch
            {
                0 => "Tiêm chủng",
                1 => "Khám sức khỏe",
                _ => "Không xác định"
            };
        }

        public static string StatusDisplay(this Campaign campaign)
        {
            return campaign.Status switch
            {
                0 => "Đã lên kế hoạch",
                1 => "Đang thực hiện",
                2 => "Đã hoàn thành",
                3 => "Đã hủy",
                _ => "Không xác định"
            };
        }
    }
}
