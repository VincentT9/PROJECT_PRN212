using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BusinessObjects;
using DataAccessLayer;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for ScheduleStudentsView.xaml
    /// </summary>
    public partial class ScheduleStudentsView : Window
    {
        private readonly ScheduleDAO _scheduleDAO = new();
        private Guid _scheduleId;
        private Schedule? _schedule;
        public ObservableCollection<StudentWithVaccinationStatus> Students { get; set; } = new();
        public ObservableCollection<StudentWithVaccinationStatus> FilteredStudents { get; set; } = new();

        public ScheduleStudentsView(Guid scheduleId)
        {
            InitializeComponent();
            _scheduleId = scheduleId;
            StudentsDataGrid.ItemsSource = FilteredStudents;
            LoadScheduleInfo();
            LoadStudents();
            UpdateStatistics();
        }

        private void LoadScheduleInfo()
        {
            _schedule = _scheduleDAO.GetSchedules().FirstOrDefault(s => s.Id == _scheduleId);
            if (_schedule != null)
            {
                ScheduleDateText.Text = _schedule.ScheduledDate.ToString("dd/MM/yyyy");
                LocationText.Text = _schedule.Location ?? "N/A";
                HeaderText.Text = $"Danh sách học sinh - {_schedule.Location} ({_schedule.ScheduledDate:dd/MM/yyyy})";
            }
        }

        private void LoadStudents()
        {
            Students.Clear();
            var studentIds = _scheduleDAO.GetStudentIdsByScheduleId(_scheduleId);
            var scheduleDetails = _scheduleDAO.GetScheduleDetailsByScheduleId(_scheduleId);
            foreach (var studentId in studentIds)
            {
                var scheduleDetail = scheduleDetails.FirstOrDefault(sd => sd.StudentId == studentId);
                string status = "Chưa tiêm";
                if (scheduleDetail?.VaccinationResult != null && !string.IsNullOrEmpty(scheduleDetail.VaccinationResult.Notes))
                    status = scheduleDetail.VaccinationResult.Notes;
                Students.Add(new StudentWithVaccinationStatus
                {
                    Id = studentId,
                    StudentCode = $"HS{studentId.ToString().Substring(0, 8)}",
                    FullName = $"Học sinh {studentId.ToString().Substring(0, 8)}",
                    Class = "Lớp 10A1",
                    DateOfBirth = DateTime.Now.AddYears(-16),
                    VaccinationStatus = status,
                    Notes = ""
                });
            }
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            FilteredStudents.Clear();
            var filtered = Students.AsEnumerable();

            // Apply search filter
            if (!string.IsNullOrEmpty(SearchBox.Text) && SearchBox.Text != "Tìm kiếm học sinh...")
            {
                filtered = filtered.Where(s => s.FullName?.Contains(SearchBox.Text, StringComparison.OrdinalIgnoreCase) == true ||
                                             s.StudentCode?.Contains(SearchBox.Text, StringComparison.OrdinalIgnoreCase) == true);
            }

            // Apply status filter
            if (StatusFilter != null && StatusFilter.SelectedIndex > 0)
            {
                var selectedStatus = StatusFilter.SelectedIndex switch
                {
                    1 => "Đã tiêm",
                    2 => "Chưa tiêm",
                    3 => "Từ chối",
                    _ => ""
                };
                filtered = filtered.Where(s => s.VaccinationStatus == selectedStatus);
            }

            foreach (var student in filtered)
            {
                FilteredStudents.Add(student);
            }
        }

        private void UpdateStatistics()
        {
            var totalStudents = Students.Count;
            var vaccinatedStudents = Students.Count(s => s.VaccinationStatus == "Đã tiêm");
            var notVaccinatedStudents = Students.Count(s => s.VaccinationStatus == "Chưa tiêm");
            var refusedStudents = Students.Count(s => s.VaccinationStatus == "Từ chối");

            TotalStudentsText.Text = totalStudents.ToString();
            VaccinatedStudentsText.Text = vaccinatedStudents.ToString();
            NotVaccinatedStudentsText.Text = notVaccinatedStudents.ToString();
            RefusedStudentsText.Text = refusedStudents.ToString();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            // In a real application, you would open a student selection dialog
            MessageBox.Show("Chức năng thêm học sinh sẽ được implement trong phần tiếp theo.", 
                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportList_Click(object sender, RoutedEventArgs e)
        {
            // In a real application, you would export to Excel or PDF
            MessageBox.Show("Chức năng xuất danh sách sẽ được implement trong phần tiếp theo.", 
                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RecordResult_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsDataGrid.SelectedItem is StudentWithVaccinationStatus selected)
            {
                // Cập nhật trạng thái tiêm chủng trên UI
                selected.VaccinationStatus = "Đã tiêm";
                // Lưu vào database
                _scheduleDAO.UpdateStudentVaccinationStatus(selected.Id, _scheduleId, "Đã tiêm");
                StudentsDataGrid.Items.Refresh();
                UpdateStatistics();
                MessageBox.Show($"Đã ghi nhận kết quả tiêm chủng cho học sinh {selected.FullName}",
                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một học sinh để ghi nhận kết quả.",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RemoveStudent_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsDataGrid.SelectedItem is StudentWithVaccinationStatus selected)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa học sinh {selected.FullName} khỏi lịch này?",
                    "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Remove student from schedule
                        Students.Remove(selected);
                        ApplyFilters();
                        UpdateStatistics();

                        MessageBox.Show("Đã xóa học sinh khỏi lịch thành công!", 
                            "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa học sinh: {ex.Message}", 
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một học sinh để xóa.", 
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

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadStudents();
            UpdateStatistics();
        }
    }

    // Helper class to display student with vaccination status
    public class StudentWithVaccinationStatus
    {
        public Guid Id { get; set; }
        public string StudentCode { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Class { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public string VaccinationStatus { get; set; } = "";
        public string Notes { get; set; } = "";
    }
} 