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
using Services.Interface;
using Services;
using BusinessObjects;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for ParentalConsentView.xaml
    /// </summary>
    public partial class ParentalConsentView : Window
    {
        private readonly ICampaignService _campaignService = new CampaignService();
        private readonly IStudentService _studentService = new StudentService();
        private readonly IConsentFormService _consentFormService = new ConsentFormService();

        private Guid _parentUserId;
        public ParentalConsentView(Guid parentUserId)
        {
            InitializeComponent();
            _parentUserId = parentUserId;

            LoadCampaigns();
            LoadStudents();
        }

        private void LoadStudents()
        {
            var students = _studentService.GetStudentsByParentId(_parentUserId); //lọc ra những học sinh là con của phụ huynh này
            StudentComboBox.ItemsSource = null;
            StudentComboBox.ItemsSource = students;
        }

        private void LoadCampaigns()
        {
            var campaigns = _campaignService.GetAllCampaigns(); // lọc status = InProgress để phụ huynh chọn
            CampaignComboBox.ItemsSource = null;
            CampaignComboBox.ItemsSource = campaigns;
        }

        private void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CampaignComboBox.SelectedItem is not Campaign campaign ||
                StudentComboBox.SelectedItem is not Student student)
            {
                MessageBox.Show("Vui lòng chọn chiến dịch và học sinh.");
                return;
            }
            var isExist = _consentFormService.IsExists(student.Id, campaign.Id);
            if (isExist != null)
            {
                isExist.ConsentDate = DateTime.Now;
                isExist.ReasonForDecline = null;// xóa đi lý do nếu có
                isExist.UpdateAt = DateTime.Now; //cập nhật lại thời gian
                isExist.IsApproved = true; //update lại đồng ý
                var result = _consentFormService.UpdateConsentForm(isExist);
                if (result)
                {
                    MessageBox.Show("Đã gửi đồng ý thành công.");
                    this.Close();
                    return;
                }
                MessageBox.Show("Gửi form thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
            else
            {
                var result2 = _consentFormService.AddNewConsentForm(new ConsentForm
                {
                    Id = Guid.NewGuid(),
                    CampaignId = campaign.Id,
                    StudentId = student.Id,
                    IsApproved = true,
                    ConsentDate = DateTime.Now,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                });
                if (result2)
                {
                    MessageBox.Show("Đã gửi đồng ý thành công.");
                    this.Close();
                    return;
                }
                MessageBox.Show("Gửi form thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            if (CampaignComboBox.SelectedItem is not Campaign campaign ||
                StudentComboBox.SelectedItem is not Student student)
            {
                MessageBox.Show("Vui lòng chọn chiến dịch và học sinh.");
                return;
            }

            var isExist = _consentFormService.IsExists(student.Id, campaign.Id);
            if (isExist != null)
            {
                var reason = DeclineReasonTextBox.Text?.Trim();
                isExist.ConsentDate = DateTime.Now; //cập nhật lại thời gian
                isExist.ReasonForDecline = reason; //Thêm lý do từ chối
                isExist.IsApproved = false;//update lại từ chối
                var result = _consentFormService.UpdateConsentForm(isExist);
                if (result)
                {
                    MessageBox.Show("Đã gửi từ chối thành công.");
                    this.Close();
                    return;
                }
                MessageBox.Show("Gửi form thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
            else
            {
                var reason2 = DeclineReasonTextBox.Text?.Trim();
                var result2 = _consentFormService.AddNewConsentForm(new ConsentForm
                {
                    Id = Guid.NewGuid(),
                    CampaignId = campaign.Id,
                    StudentId = student.Id,
                    IsApproved = false,
                    ReasonForDecline = reason2,
                    ConsentDate = DateTime.Now,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                });
                if (result2)
                {
                    MessageBox.Show("Đã gửi từ chối thành công.");
                    this.Close();
                    return;
                }
                MessageBox.Show("Gửi form thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
                
        }
    }
}
