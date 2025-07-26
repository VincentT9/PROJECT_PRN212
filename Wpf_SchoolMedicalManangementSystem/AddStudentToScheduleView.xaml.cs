using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BusinessObjects;
using DataAccessLayer;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class AddStudentToScheduleView : Window
    {
        private readonly StudentDAO _studentDAO = new();
        private readonly ScheduleDAO _scheduleDAO = new();
        private readonly Guid _scheduleId;
        public ObservableCollection<SelectableStudent> Students { get; set; } = new();
        public List<Student> SelectedStudents => Students.Where(s => s.IsSelected).Select(s => s.Student).ToList();

        public AddStudentToScheduleView(Guid scheduleId)
        {
            InitializeComponent();
            _scheduleId = scheduleId;
            dgStudents.ItemsSource = Students;
            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                Students.Clear();
                
                // Get all students from database
                var allStudents = _studentDAO.GetAllStudents();
                
                // Get students already in this schedule
                var existingStudentIds = _scheduleDAO.GetStudentIdsByScheduleId(_scheduleId);
                
                // Add students not already in the schedule
                foreach (var student in allStudents.Where(s => !existingStudentIds.Contains(s.Id)))
                {
                    Students.Add(new SelectableStudent
                    {
                        IsSelected = false,
                        Student = student,
                        StudentCode = student.StudentCode,
                        FullName = student.FullName,
                        Class = student.Class,
                        Gender = GetGenderDisplay(student.Gender),
                        DateOfBirth = student.DateOfBirth
                    });
                }
                
                UpdateSelectedCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách học sinh: {ex.Message}", 
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private string GetGenderDisplay(int? gender)
        {
            return gender switch
            {
                0 => "Nam",
                1 => "Nữ",
                2 => "Khác",
                _ => ""
            };
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (textBox.Text == "Tìm kiếm học sinh theo tên, mã học sinh hoặc lớp")
            {
                textBox.Text = string.Empty;
                textBox.FontStyle = FontStyles.Normal;
                textBox.Foreground = Brushes.Black;
            }
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Tìm kiếm học sinh theo tên, mã học sinh hoặc lớp";
                textBox.FontStyle = FontStyles.Italic;
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (textBox.Text == "Tìm kiếm học sinh theo tên, mã học sinh hoặc lớp")
                return;

            var searchText = textBox.Text.Trim().ToLower();
            var view = dgStudents.ItemsSource as ObservableCollection<SelectableStudent>;
            
            if (string.IsNullOrEmpty(searchText))
            {
                dgStudents.ItemsSource = Students;
            }
            else
            {
                var filteredResults = Students.Where(s => 
                    s.FullName.ToLower().Contains(searchText) ||
                    s.StudentCode.ToLower().Contains(searchText) ||
                    s.Class.ToLower().Contains(searchText)).ToList();
                
                dgStudents.ItemsSource = new ObservableCollection<SelectableStudent>(filteredResults);
            }
        }

        private void UpdateSelectedCount()
        {
            var count = Students.Count(s => s.IsSelected);
            txtSelectedCount.Text = count.ToString();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnAddStudents_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedStudents = SelectedStudents;
                if (!selectedStudents.Any())
                {
                    MessageBox.Show("Vui lòng chọn ít nhất một học sinh để thêm vào lịch.",
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
                // Add selected students to schedule
                foreach (var student in selectedStudents)
                {
                    var scheduleDetail = new ScheduleDetail
                    {
                        Id = Guid.NewGuid(),
                        ScheduleId = _scheduleId,
                        StudentId = student.Id,
                        VaccinationDate = DateTime.Now,
                        CreateAt = DateTime.Now,
                        UpdateAt = DateTime.Now
                    };
                    
                    _scheduleDAO.AddStudentToSchedule(scheduleDetail);
                }
                
                MessageBox.Show($"Đã thêm {selectedStudents.Count} học sinh vào lịch thành công!",
                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm học sinh: {ex.Message}",
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    
    public class SelectableStudent
    {
        public bool IsSelected { get; set; }
        public Student Student { get; set; }
        public string StudentCode { get; set; }
        public string FullName { get; set; }
        public string Class { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
