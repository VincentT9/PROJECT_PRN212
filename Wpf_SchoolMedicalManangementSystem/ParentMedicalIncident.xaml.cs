using BusinessObjects;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SchoolMedicalManagementSystem.Enum;
using Services; // Assuming your enums are here

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for ParentMedicalIncident.xaml
    /// </summary>
    public partial class ParentMedicalIncident : UserControl
    {
        private readonly IStudentService _studentService;
        private readonly IMedicalIncidentService _medicalIncidentService;
        private User? currentUser;
        private List<Student> childrens;

        public ObservableCollection<MedicalIncidentViewModel> MedicalIncidents { get; set; }

        public ParentMedicalIncident()
        {
            InitializeComponent();

            
            _studentService = new StudentService(); 
            _medicalIncidentService = new MedicalIncidentService();

            MedicalIncidents = new ObservableCollection<MedicalIncidentViewModel>();
            this.DataContext = this;

            LoadParentAndIncidents();
        }

        private async void LoadParentAndIncidents()
        {
            currentUser = App.Current.Properties["CurrentUser"] as BusinessObjects.User;

            if (currentUser == null)
            {
                MessageBox.Show("Không tìm thấy thông tin người dùng. Vui lòng đăng nhập lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            childrens =  _studentService.GetStudentsByParentId(currentUser.Id);

            MedicalIncidents.Clear(); 

            foreach (var child in childrens)
            {
                var incidentsForChild = await _medicalIncidentService.GetMedicalIncidentsByStudentIdAsync(child.Id);
                if (incidentsForChild != null)
                {
                    foreach (var incident in incidentsForChild)
                    {
                        MedicalIncidents.Add(new MedicalIncidentViewModel(incident));
                    }
                }
            }
        }

        private async void Detail_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                
                MedicalIncidentViewModel selectedIncidentViewModel = btn.DataContext as MedicalIncidentViewModel;

                if (selectedIncidentViewModel != null)
                {
                    
                    MedicalIncident fullIncident = await _medicalIncidentService.GetMedicalIncidentByIdAsync(selectedIncidentViewModel.Id);

                    if (fullIncident != null)
                    {
                        
                        DetailMedicalIncidentWindow detailWindow = new DetailMedicalIncidentWindow(fullIncident);
                        detailWindow.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy chi tiết sự kiện y tế.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }

    public partial class MedicalIncidentViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        public MedicalIncident OriginalIncident { get; private set; }

        public MedicalIncidentViewModel(MedicalIncident incident)
        {
            OriginalIncident = incident;
            UpdateDisplayProperties();
        }

        public Guid Id => OriginalIncident.Id;
        public DateTime IncidentDate => OriginalIncident.IncidentDate;
        public string DisplayCreateAt
        {
            get
            {
                if (OriginalIncident.CreateAt == DateTime.MinValue || OriginalIncident.CreateAt == DateTime.MaxValue)
                {
                    return "Không xác định"; 
                }
                return OriginalIncident.CreateAt.ToString("dd/MM/yyyy HH:mm");
            }
        }

        public string StudentName => OriginalIncident.Student?.FullName ?? "N/A";

        public string Description => OriginalIncident.Description ?? "Không có mô tả";
        public string ActionsTaken => OriginalIncident.ActionsTaken ?? "Chưa có hành động";

        public string IncidentTypeDisplay
        {
            get
            {
                if (System.Enum.IsDefined(typeof(IncidentType), OriginalIncident.IncidentType))
                {
                    return ((IncidentType)OriginalIncident.IncidentType).ToString();
                }
                return "Không xác định";
            }
        }

        public string StatusDisplay
        {
            get
            {
                if (System.Enum.IsDefined(typeof(IncidentStatus), OriginalIncident.Status))
                {
                    return ((IncidentStatus)OriginalIncident.Status).ToString();
                }
                return "Không xác định";
            }
        }

        public string StudentCode => OriginalIncident.Student?.StudentCode ?? "N/A";
        public string StudentClass => $"{OriginalIncident.Student?.Class ?? "N/A"} | {OriginalIncident.Student?.SchoolYear ?? "N/A"}"; // Assuming ClassName and AcademicYear exist
        public string StudentDateOfBirth => OriginalIncident.Student?.DateOfBirth.ToString("dd/MM/yyyy") ?? "N/A";
        public string Outcome => OriginalIncident.Outcome ?? "Chưa thông báo";


        private void UpdateDisplayProperties()
        {
            OnPropertyChanged(nameof(DisplayCreateAt));
            OnPropertyChanged(nameof(IncidentTypeDisplay));
            OnPropertyChanged(nameof(StatusDisplay));
            OnPropertyChanged(nameof(StudentName));
            OnPropertyChanged(nameof(StudentCode));
            OnPropertyChanged(nameof(StudentClass));
            OnPropertyChanged(nameof(StudentDateOfBirth));
            OnPropertyChanged(nameof(Outcome));
        }
    }
}