using BusinessObjects;
using System.Windows;
using SchoolMedicalManagementSystem.Enum; 

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class DetailMedicalIncidentWindow : Window
    {
        public MedicalIncidentViewModel DetailIncident { get; set; }

        public DetailMedicalIncidentWindow(MedicalIncident incident)
        {
            InitializeComponent();
            DetailIncident = new MedicalIncidentViewModel(incident);
            this.DataContext = DetailIncident;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the detail window
        }
    }
}