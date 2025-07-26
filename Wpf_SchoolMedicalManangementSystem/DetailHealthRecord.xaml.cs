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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Services;
using BusinessObjects;
using Services.Interface;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for DetailHealthRecord.xaml
    /// </summary>
    public partial class DetailHealthRecord : UserControl
    {
        private readonly IStudentService _studentService = new StudentService();
        private readonly IHealthRecordService _healthRecordService = new HealthRecordService();
        public DetailHealthRecord(Guid studentId)
        {
            InitializeComponent();
            var student = _studentService.GetStudentById(studentId);
            var healthRecord = _healthRecordService.GetHealthRecordByStudentIdAsync(studentId);
            this.DataContext = new DetailHealthRecordViewModel(student, healthRecord);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as DetailHealthRecordViewModel;
            if (vm != null && vm.HealthRecord != null)
            {
                var editWindow = new EditHealthRecordDialog(vm.HealthRecord); // Sẽ tạo EditHealthRecordDialog kế thừa Window
                if (editWindow.ShowDialog() == true)
                {
                    // Sau khi lưu thành công, reload lại dữ liệu
                    var studentId = vm.Student.Id;
                    var student = _studentService.GetStudentById(studentId);
                    var healthRecord = _healthRecordService.GetHealthRecordByStudentIdAsync(studentId);
                    this.DataContext = new DetailHealthRecordViewModel(student, healthRecord);
                }
            }
        }
    }

    public class DetailHealthRecordViewModel
    {
        public Student Student { get; set; }
        public HealthRecord HealthRecord { get; set; }
        public DetailHealthRecordViewModel(Student student, HealthRecord healthRecord)
        {
            Student = student;
            HealthRecord = healthRecord;
        }
    }
}
