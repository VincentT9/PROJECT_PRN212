using BusinessObjects;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class MedicalIncidentSuppliesView : Window
    {
        public ObservableCollection<MedicalSupplyUsage> SuppliesUsed { get; set; }

        public MedicalIncidentSuppliesView(MedicalIncident incident, List<MedicalSupplyUsage> usages)
        {
            InitializeComponent();
            Title = $"Vật tư đã sử dụng - {incident.Student?.FullName} ({incident.IncidentDate:dd/MM/yyyy})";
            SuppliesUsed = new ObservableCollection<MedicalSupplyUsage>(usages);
            DataContext = this;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
