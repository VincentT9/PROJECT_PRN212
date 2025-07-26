using BusinessObjects;
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

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for ParentDashboar.xaml
    /// </summary>
    public partial class ParentDashboar : Window
    {
        User? currentUser;
        public ParentDashboar()
        {         
            InitializeComponent();
            currentUser = App.Current.Properties["CurrentUser"] as BusinessObjects.User;
            if (currentUser != null)
            {
                txtUsername.Text = currentUser.Username;
            }
            MainContent.Content = new ParentDashboardHomeView(); // Luôn hiển thị trang chủ khi khởi động


        }

        private void HealthProfile_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new HealthRecordWindow();
        }

        private void MedicalEvents_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ParentMedicalIncident();
        }

        private void SendMedicine_Click(object sender, RoutedEventArgs e)
        {
            ParentMedicationRequest parentMedicationRequest = new ParentMedicationRequest();
            MainContent.Content = parentMedicationRequest;
        }

        private void Notifications_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Đi tới Thông báo");
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            // Nếu chưa ở trang chủ thì chuyển về trang chủ
            if (MainContent.Content is not ParentDashboardHomeView)
            {
                MainContent.Content = new ParentDashboardHomeView();
            }
        }
    }
}
