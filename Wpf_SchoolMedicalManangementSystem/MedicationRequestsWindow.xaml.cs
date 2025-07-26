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
using SchoolMedicalManagementSystem.Enum;
using Services;
using Services.Interface;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for MedicationRequestsWindow.xaml
    /// </summary>
    public partial class MedicationRequestsWindow : Window
    {
        IMedicationRequestService _medicationRequestService = new MedicationRequestService();
        public MedicationRequestsWindow()
        {
            InitializeComponent();
        }

        private void LoadToday_Click(object sender, RoutedEventArgs e)
        {
            var data = _medicationRequestService.GetTodayMedications();
            foreach (var item in data)
            {
                item.StatusText = item.Status.ToString();
            }
            lvMedications.ItemsSource = data;
        }

        private void LoadConfirmed_Click(object sender, RoutedEventArgs e)
        {
            var data = _medicationRequestService.GetByStatus(RequestStatus.Received);
            foreach (var item in data)
            {
                item.StatusText = item.Status.ToString();
            }
            lvMedications.ItemsSource = data;
        }

        private void LoadPending_Click(object sender, RoutedEventArgs e)
        {
            var data = _medicationRequestService.GetByStatus(RequestStatus.Pending);
            foreach (var item in data)
            {
                item.StatusText = item.Status.ToString();
            }
            lvMedications.ItemsSource = data;
        }

        private void LoadOverdue_Click(object sender, RoutedEventArgs e)
        {
            var data = _medicationRequestService.GetOverdueOrDone();
            foreach (var item in data)
            {
                item.StatusText = item.Status.ToString();
            }
            lvMedications.ItemsSource = data;
        }
    }
}
