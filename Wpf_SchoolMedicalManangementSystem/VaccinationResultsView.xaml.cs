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
using Services;
using Services.Interface;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for VaccinationResultsView.xaml
    /// </summary>
    public partial class VaccinationResultsView : Window
    {
        private readonly IVaccinationResultService _vaccinationResultService = new VaccinationResultService();
        private readonly IStudentService _studentService = new StudentService();
        public VaccinationResultsView()
        {
            InitializeComponent();
            LoadStudents();
        }
        private void LoadStudents()
        {
            var students = _studentService.GetAllStudents(); 
            StudentListBox.ItemsSource = students;
        }

        private void StudentListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedStudent = StudentListBox.SelectedItem as Student;
            if (selectedStudent == null) return;

            var results = _vaccinationResultService.GetByStudentId(selectedStudent.Id);
            VaccinationResultListBox.ItemsSource = results;

            // Reset detail
            DosageTextBlock.Text = SideEffectsTextBlock.Text = NotesTextBlock.Text = "";
            CreateAtTextBlock.Text = UpdateAtTextBlock.Text = VaccinationDateTextBlock.Text = "";
        }

        private void VaccinationResultListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedResult = VaccinationResultListBox.SelectedItem as VaccinationResult;
            if (selectedResult == null) return;

            DosageTextBlock.Text = selectedResult.DosageGiven;
            SideEffectsTextBlock.Text = selectedResult.SideEffects;
            NotesTextBlock.Text = selectedResult.Notes;
            CreateAtTextBlock.Text = selectedResult.CreateAt.ToString("dd/MM/yyyy");
            UpdateAtTextBlock.Text = selectedResult.UpdateAt.ToString("dd/MM/yyyy");

            var vaccinationDate = selectedResult.ScheduleDetail?.VaccinationDate;
            VaccinationDateTextBlock.Text = vaccinationDate?.ToString("dd/MM/yyyy") ?? "(Không rõ)";
        }
    }
}
