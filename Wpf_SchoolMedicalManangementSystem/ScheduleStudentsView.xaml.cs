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
        private readonly StudentDAO _studentDAO = new();
        private readonly bool _isMedicalStaff;
        private Guid _scheduleId;
        private Schedule? _schedule;
        public ObservableCollection<StudentWithVaccinationStatus> Students { get; set; } = new();
        public ObservableCollection<StudentWithVaccinationStatus> FilteredStudents { get; set; } = new();

        public ScheduleStudentsView(Guid scheduleId, bool isMedicalStaff = false)
        {
            InitializeComponent();
            _scheduleId = scheduleId;
            _isMedicalStaff = isMedicalStaff;
            
            if (_isMedicalStaff)
            {
                btnAddStudent.Visibility = Visibility.Collapsed;
                
            }
            
            StudentsDataGrid.ItemsSource = FilteredStudents;
            LoadScheduleInfo();
            LoadStudents();
            UpdateStatistics();
        }

        private void LoadScheduleInfo()
        {
            _schedule = _scheduleDAO.GetScheduleByScheduleId(_scheduleId);
            if (_schedule != null)
            {
                ScheduleDateText.Text = _schedule.ScheduledDate.ToString("dd/MM/yyyy");
                LocationText.Text = _schedule.Location ?? "N/A";
                HeaderText.Text = $"Danh sách học sinh - {_schedule.Location} ({_schedule.ScheduledDate:dd/MM/yyyy})";
            }
        }
        
        private void LoadStudents()
        {
            try
            {
                Students.Clear();
                var studentIds = _scheduleDAO.GetStudentIdsByScheduleId(_scheduleId);
                var scheduleDetails = _scheduleDAO.GetScheduleDetailsByScheduleId(_scheduleId);
                
                foreach (var studentId in studentIds)
                {
                    var student = _studentDAO.GetStudentById(studentId);
                    if (student == null) continue;
                    
                    var scheduleDetail = scheduleDetails.FirstOrDefault(sd => sd.StudentId == studentId);
                    string updatedBy = "Chưa ghi nhận";
                    
                    if (scheduleDetail != null)
                    {
                        if (scheduleDetail.VaccinationResult != null && !string.IsNullOrEmpty(scheduleDetail.VaccinationResult.UpdatedBy))
                        {
                            updatedBy = scheduleDetail.VaccinationResult.UpdatedBy;
                        }
                        else if (scheduleDetail.HealthCheckupResult != null && !string.IsNullOrEmpty(scheduleDetail.HealthCheckupResult.UpdatedBy))
                        {
                            updatedBy = scheduleDetail.HealthCheckupResult.UpdatedBy;
                        }
                    }
                    
                    Students.Add(new StudentWithVaccinationStatus
                    {
                        Id = studentId,
                        StudentCode = student.StudentCode ?? $"HS{studentId.ToString().Substring(0, 8)}",
                        FullName = student.FullName ?? $"Học sinh {studentId.ToString().Substring(0, 8)}",
                        Class = student.Class ?? "Chưa có lớp",
                        DateOfBirth = student.DateOfBirth,
                        UpdatedBy = updatedBy
                    });
                }
                
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách học sinh: {ex.Message}", 
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilters()
        {
            FilteredStudents.Clear();
            var filtered = Students.AsEnumerable();

            if (!string.IsNullOrEmpty(SearchBox.Text) && SearchBox.Text != "Tìm kiếm học sinh...")
            {
                filtered = filtered.Where(s => 
                    s.FullName?.Contains(SearchBox.Text, StringComparison.OrdinalIgnoreCase) == true ||
                    s.StudentCode?.Contains(SearchBox.Text, StringComparison.OrdinalIgnoreCase) == true ||
                    s.UpdatedBy?.Contains(SearchBox.Text, StringComparison.OrdinalIgnoreCase) == true);
            }

            foreach (var student in filtered)
            {
                FilteredStudents.Add(student);
            }
        }

        private void UpdateStatistics()
        {
            var totalStudents = Students.Count;
            TotalStudentsText.Text = totalStudents.ToString();
            
            VaccinatedStudentsText.Text = "0";
            NotVaccinatedStudentsText.Text = totalStudents.ToString();
            RefusedStudentsText.Text = "0";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
   
            if (!LoginWindow.IsAdmin())
            {
                MessageBox.Show("Bạn không có quyền thêm học sinh vào lịch tiêm chủng.",
                    "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            var addStudentWindow = new AddStudentToScheduleView(_scheduleId);
            if (addStudentWindow.ShowDialog() == true)
            {
                // Refresh the student list
                LoadStudents();
                UpdateStatistics();
                MessageBox.Show("Đã cập nhật danh sách học sinh.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExportList_Click(object sender, RoutedEventArgs e)
        {
           
            MessageBox.Show("Chức năng xuất danh sách sẽ được implement trong phần tiếp theo.", 
                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RecordResult_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsDataGrid.SelectedItem is StudentWithVaccinationStatus selected)
            {
                try
                {
                    
                    var schedule = _scheduleDAO.GetScheduleByScheduleId(_scheduleId);
                    if (schedule == null)
                    {
                        MessageBox.Show("Không tìm thấy thông tin lịch.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    
                    var campaign = schedule.Campaign;
                    if (campaign == null)
                    {
                        MessageBox.Show("Không tìm thấy thông tin chiến dịch.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    
                    var student = _studentDAO.GetStudentById(selected.Id);
                    if (student == null)
                    {
                        MessageBox.Show("Không tìm thấy thông tin học sinh.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    bool result = false;
                    
                    if (campaign.Type == 0) // Vaccination
                    {
                        var form = new VaccinationResultForm(student, _scheduleId);
                        result = form.ShowDialog() ?? false;
                    }
                    else if (campaign.Type == 1) // Health Checkup
                    {
                        var form = new HealthCheckupResultForm(student, _scheduleId);
                        result = form.ShowDialog() ?? false;
                    }
                    else
                    {
                        MessageBox.Show($"Loại chiến dịch không hợp lệ: {campaign.Type}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    
                    if (result)
                    {
                        LoadStudents();
                        UpdateStatistics();
                        MessageBox.Show("Đã ghi nhận kết quả thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi mở form ghi nhận kết quả: {ex.Message}", 
                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một học sinh để ghi nhận kết quả.",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RemoveStudent_Click(object sender, RoutedEventArgs e)
        {
            if (!LoginWindow.IsAdmin())
            {
                MessageBox.Show("Bạn không có quyền xóa học sinh khỏi lịch tiêm chủng.",
                    "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if (StudentsDataGrid.SelectedItem is StudentWithVaccinationStatus selected)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa học sinh {selected.FullName} khỏi lịch này?",
                    "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
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

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadStudents();
            UpdateStatistics();
        }
    }
    public class StudentWithVaccinationStatus
    {
        public Guid Id { get; set; }
        public string StudentCode { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Class { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public string UpdatedBy { get; set; } = "";
        
        public string CreatedBy { get { return UpdatedBy; } }
    }
} 