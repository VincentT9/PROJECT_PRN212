using BusinessObjects;
using System.Windows;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class DetailMedicationRequestWindow : Window
    {
        public DetailMedicationRequestWindow(MedicationRequest request)
        {
            InitializeComponent();
            this.DataContext = request;
        }
    }
} 