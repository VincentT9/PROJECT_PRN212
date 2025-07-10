using BusinessObjects;
using SchoolMedicalManagementSystem.Enum;
using Services;
using Services.Interface;
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
    /// Interaction logic for ParentMedicationRequest.xaml
    /// </summary>
    public partial class ParentMedicationRequest : Window
    {
        private readonly IMedicationRequestService _medicationRequestService;
        private string _imageBase64; // Lưu chuỗi Base64 của ảnh
        public ParentMedicationRequest()
        {
            InitializeComponent();
            _medicationRequestService = new MedicationRequestService();
            dateSend.DisplayDateStart = DateTime.Now; // Chỉ cho chọn từ ngày mai trở đi
            dateSend.SelectedDate = DateTime.Now;     // Gợi ý mặc định là ngày mai
            dateSend.SelectedDateChanged += dateSend_SelectedDateChanged;           
        }

        // Upload image to base64 string
        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                string sourceFilePath = openFileDialog.FileName;
                try
                {
                    byte[] imageBytes = System.IO.File.ReadAllBytes(sourceFilePath);
                    _imageBase64 = Convert.ToBase64String(imageBytes);

                    // Hiển thị ảnh lên Image control
                    using (var ms = new System.IO.MemoryStream(imageBytes))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = ms;
                        bitmap.EndInit();
                        imgPreview.Source = bitmap;
                        imgPreview.Visibility = Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi đọc ảnh: {ex.Message}");
                    _imageBase64 = null;
                    imgPreview.Source = null;
                    imgPreview.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                imgPreview.Source = null;
                imgPreview.Visibility = Visibility.Collapsed;
            }
        }

        //Check date is in the future
        private void dateSend_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dateSend.SelectedDate.HasValue)
            {
                var selectedDate = dateSend.SelectedDate.Value.Date;
                var today = DateTime.Now.Date;
                if (selectedDate <= today)
                {
                    MessageBox.Show("Vui lòng chọn ngày bắt đầu là ngày trong tương lai!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    dateSend.SelectedDate = null;
                }
            }
        }

        // Send request to database
        private void btnSendRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MedicationRequest medicationRequest = new MedicationRequest();
                medicationRequest.StudentId = Guid.Parse(cboStudent.SelectedValue.ToString());
                medicationRequest.MedicationName = txtMedicationName.Text;
                medicationRequest.Dosage = short.Parse(txtDosage.Text);
                medicationRequest.ImagesMedicalInvoice = _imageBase64; // Lưu Base64 vào database
                medicationRequest.NumberOfDayToTake = short.Parse(txtQuantity.Text);
                medicationRequest.StartDate = dateSend.SelectedDate;
                medicationRequest.EndDate = dateSend.SelectedDate.Value.AddDays(short.Parse(txtQuantity.Text) - 1); // Tính ngày kết thúc
                medicationRequest.Instructions = txtNote.Text;
                medicationRequest.Status = (int)RequestStatus.Pending; // Trạng thái mặc định là Pending
                                                                  //Bổ sung lấy thông tin người dùng hiện tại làm cho trường CreatedBy
                _medicationRequestService.CreateMedicationRequest(medicationRequest);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi yêu cầu: {ex.Message}",
                                 "Lỗi",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Error);
                return;
            }
        }
    }
}
