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
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Interface;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for AddStudentToScheduleView.xaml
    /// </summary>
    public partial class AddStudentToScheduleView : Window
    {
        IScheduleDetailService _scheduleDetailService = new ScheduleDetailService();
        IScheduleService _scheduleService = new ScheduleService();
        IStudentService _studentService = new StudentService();
        public AddStudentToScheduleView()
        {
            InitializeComponent();
            LoadSchedules();
        }

        private void LoadSchedules()
        {
            var list = _scheduleService.GetAllSchedules();
            if (list != null)
            {
                ScheduleComboBox.ItemsSource = null;
                ScheduleComboBox.ItemsSource = list;
            }
        }
        private void ScheduleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSchedule = ScheduleComboBox.SelectedItem as Schedule;

            if (selectedSchedule != null)
            {
                LoadStudentsInSchedule();
                LoadStudentsNotInSchedule();
            }
        }
        private void LoadStudentsInSchedule()
        {
            var item = ScheduleComboBox.SelectedItem as Schedule;
            var students = _scheduleDetailService.GetStudentsByScheduleId(item.Id);
            StudentListView.ItemsSource = null;
            StudentListView.ItemsSource = students;
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedStudent = StudentComboBox.SelectedItem as Student;
            var selectedSchedule = ScheduleComboBox.SelectedItem as Schedule;
            if (selectedSchedule == null || selectedStudent == null)
            {
                MessageBox.Show("Vui lòng chọn lịch trình và học sinh.");
                return;
            }

            if (_scheduleDetailService.ExistsInSchedule(selectedStudent.Id, selectedSchedule.Id))
            {
                MessageBox.Show("Học sinh đã có trong lịch trình.");
                return;
            }

            var isSuccess = _scheduleDetailService.AddScheduleDetail(selectedStudent.Id, selectedSchedule.Id);
            if(!isSuccess)
            {
                MessageBox.Show("Không thể thêm học sinh vào lịch trình. Vui lòng kiểm tra lại thông tin.");
                return;
            }
            // Cập nhật lại giao diện
            LoadStudentsInSchedule();
            LoadStudentsNotInSchedule();
            MessageBox.Show("Đã thêm học sinh vào lịch trình!");
        }
        private void LoadStudentsNotInSchedule()
        {
            var selectedSchedule = ScheduleComboBox.SelectedItem as Schedule;
            if (selectedSchedule == null)
                return;

            var students = _studentService.GetStudentsNotInSchedule(selectedSchedule.Id);
            StudentComboBox.ItemsSource = null;
            StudentComboBox.ItemsSource = students;
        }
    }
}
