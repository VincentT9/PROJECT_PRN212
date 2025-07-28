using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using BusinessObjects;
using DataAccessLayer;
using SchoolMedicalManagementSystem.Enum;
using Services;
using Services.Interface;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for MedicalSupplyView.xaml
    /// </summary>
    public partial class MedicalSupplyView : UserControl
    {

        IMedicationRequestService medicationRequestService = new MedicationRequestService();
        IMedicalDiaryService medicalDiaryService = new MedicalDiaryService();
        public MedicalSupplyView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            lvRequests.ItemsSource = null;
            lvRequests1.ItemsSource = null;
            lvRequests2.ItemsSource = null;
            lvRequests3.ItemsSource = null;
            var lv = medicationRequestService.GetByRequestStatusAndDiaries(RequestStatus.Received);
            foreach(var item in lv)
            {
                item.StatusText = item.Status switch
                {
                    RequestStatus.Pending => "Đang chờ",
                    RequestStatus.Received => "Đã nhận",
                    _ => "Không xác định"
                };
            }
            var lv1 = medicationRequestService.GetByStatus(RequestStatus.Received);
            foreach (var item in lv1)
            {
                item.StatusText = item.Status switch
                {
                    RequestStatus.Pending => "Đang chờ",
                    RequestStatus.Received => "Đã nhận",
                    _ => "Không xác định"
                };
            }
            var lv2 = medicationRequestService.GetByStatus(RequestStatus.Pending);
            foreach (var item in lv2)
            {
                item.StatusText = item.Status switch
                {
                    RequestStatus.Pending => "Đang chờ",
                    RequestStatus.Received => "Đã nhận",
                    _ => "Không xác định"
                };
            }
            var lv3 = medicationRequestService.GetOverdueOrDone();
            foreach (var item in lv3)
            {
                item.StatusText = item.Status switch
                {
                    RequestStatus.Pending => "Đang chờ",
                    RequestStatus.Received => "Đã nhận",
                    _ => "Không xác định"
                };
            }
            lvRequests.ItemsSource = lv;
            lvRequests1.ItemsSource = lv1;
            lvRequests2.ItemsSource = lv2;
            lvRequests3.ItemsSource = lv3;
            txtCompleted.Text = medicationRequestService.GetRequestStats().Completed.ToString();
            txtCancelled.Text = medicationRequestService.GetRequestStats().Cancelled.ToString();
            txtExpired.Text = medicationRequestService.GetRequestStats().Overdue.ToString();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag == null) return;

            Guid requestId = Guid.Parse(button.Tag.ToString());

            var request = medicationRequestService.GetMedicationRequestByGuid(requestId);
            if (request == null)
            {
                MessageBox.Show("Không tìm thấy yêu cầu thuốc.");
                return;
            }

            request.Status = RequestStatus.Received;
            medicationRequestService.UpdateMedicationRequest(request);
            MessageBox.Show("Yêu cầu thuốc đã được chấp nhận.");
            LoadData();
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag == null) return;

            Guid requestId = Guid.Parse(button.Tag.ToString());

            var request = medicationRequestService.GetMedicationRequestByGuid(requestId);
            if (request == null)
            {
                MessageBox.Show("Không tìm thấy yêu cầu thuốc.");
                return;
            }

            request.Status = RequestStatus.Returned;
            medicationRequestService.UpdateMedicationRequest(request);
            MessageBox.Show("Yêu cầu thuốc đã từ chối thành công.");
            LoadData();
        }

        private void Complete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag == null) return;

            Guid requestId = Guid.Parse(button.Tag.ToString());

            var request = medicationRequestService.GetMedicationRequestByGuid(requestId);
            if (request == null)
            {
                MessageBox.Show("Không tìm thấy yêu cầu thuốc.");
                return;
            }
            // gọi pop up
            var reasonPopup = new ReasonPopup();
            bool? result = reasonPopup.ShowDialog();

            if (result != true)
            {
                // User bấm Cancel hoặc đóng popup
                return;
            }
            string reason = reasonPopup.ReasonText;
            if (string.IsNullOrWhiteSpace(reason))
            {
                MessageBox.Show("Vui lòng nhập lý do.");
                return;
            }
            var user = LoginWindow.CurrentUser;
            // Tạo mới diary
            var diary = new MedicalDiary
            {
                Id = Guid.NewGuid(),
                MedicationReqId = request.Id,
                StudentId = request.StudentId,
                Status = (int)MedicationStatus.Taken,
                Description = reason,
                CreatedBy = user.FullName,
                UpdatedBy = user.FullName,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };
            medicalDiaryService.CreateMedicalDiary(diary);

            MessageBox.Show("Đã ghi nhận cho uống thuốc.");

            LoadData();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag == null) return;

            Guid requestId = Guid.Parse(button.Tag.ToString());

            var request = medicationRequestService.GetMedicationRequestByGuid(requestId);
            if (request == null)
            {
                MessageBox.Show("Không tìm thấy yêu cầu thuốc.");
                return;
            }
            // gọi pop up
            var reasonPopup = new ReasonPopup();
            bool? result = reasonPopup.ShowDialog();

            if (result != true)
            {
                // User bấm Cancel hoặc đóng popup
                return;
            }
            string reason = reasonPopup.ReasonText;
            if (string.IsNullOrWhiteSpace(reason))
            {
                MessageBox.Show("Vui lòng nhập lý do.");
                return;
            }
            var user = LoginWindow.CurrentUser;
            // Tạo mới diary
            var diary = new MedicalDiary
            {
                Id = Guid.NewGuid(),
                MedicationReqId = request.Id,
                StudentId = request.StudentId,
                Status = (int)MedicationStatus.Taken,
                Description = reason,
                CreatedBy = user.FullName,
                UpdatedBy = user.FullName,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };

            medicalDiaryService.CreateMedicalDiary(diary);

            MessageBox.Show("Đã ghi nhận chưa uống thuốc.");

            // Reload lại danh sách
            LoadData();
        }
    }
}
