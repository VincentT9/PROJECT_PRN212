using BusinessObjects;

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class MedicalEventTestView : Page
    {
        public ObservableCollection<MedicalIncident> TestIncidents { get; set; }

        public MedicalEventTestView()
        {
            InitializeComponent();
            DataContext = this;

           
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dữ liệu test đã được tải thành công!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
