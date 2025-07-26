using System.Windows;
using System.Windows.Controls;
using Services;
using BusinessObjects;
using Services.Interface;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class HealthRecordWindow : UserControl
    {     
        private readonly IHealthRecordService _healthRecordService = new HealthRecordService();
        private readonly IStudentService _studentService = new StudentService();
        User? currentUser;
        private List<Student> childrens;
        private List<HealthRecord> healthRecords;

        public HealthRecordWindow()
        {
            InitializeComponent();
            currentUser = App.Current.Properties["CurrentUser"] as BusinessObjects.User;
            // Lấy danh sách học sinh của phụ huynh
            childrens = _studentService.GetStudentsByParentId(currentUser.Id);
            // Lấy health record của từng học sinh
            healthRecords = childrens
                .Select(child => _healthRecordService.GetHealthRecordByStudentIdAsync(child.Id))
                .Where(hr => hr != null)
                .ToList();
            if(healthRecords.Count == 0)
            {
                MainContent.Content = new NoHealthRecordView();
            }
            else
            {
                // Show DetailHealthRecord của học sinh đầu tiên
                MainContent.Content = new DetailHealthRecord(childrens[0].Id);
            }
            // Binding danh sách học sinh vào ItemsControl
            icStudents.ItemsSource = childrens;
        }

        private void StudentTab_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var student = btn?.DataContext as Student;
            if (student != null)
            {
                var healthRecord = _healthRecordService.GetHealthRecordByStudentIdAsync(student.Id);
                if (healthRecord != null)
                {
                    MainContent.Content = new DetailHealthRecord(student.Id);
                }
                else
                {
                    MainContent.Content = new NoHealthRecordView();
                }
            }
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            // Chuyển sang giao diện thêm học sinh
            var parent = Window.GetWindow(this) as ParentDashboar;
            if (parent != null)
            {
                parent.MainContent.Content = new AddStudentHealthRecordWindow();
            }
        }

        private void DeclareHealth_Click(object sender, RoutedEventArgs e)
        {
            // Chuyển sang giao diện khai báo sức khỏe (giống thêm học sinh)
            var parent = Window.GetWindow(this) as ParentDashboar;
            if (parent != null)
            {
                parent.MainContent.Content = new AddStudentHealthRecordWindow();
            }
        }
    }

    public class StudentViewModel
    {
        public string Name { get; set; }
        public HealthRecordViewModel HealthRecord { get; set; }
    }

    public class HealthRecordViewModel
    {
        public string StudentName { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string BloodType { get; set; }
        public string Allergies { get; set; }
        public string ChronicDiseases { get; set; }
        public string PastMedicalHistory { get; set; }
        public string VisionLeft { get; set; }
        public string VisionRight { get; set; }
        public string HearingLeft { get; set; }
        public string HearingRight { get; set; }
        public string VaccinationHistory { get; set; }
        public string OtherNotes { get; set; }
    }
}
