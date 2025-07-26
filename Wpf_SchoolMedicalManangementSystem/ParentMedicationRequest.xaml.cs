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
    public partial class ParentMedicationRequest : UserControl
    {
        private readonly IMedicationRequestService _medicationRequestService;
        private List<string> _imageBase64List = new List<string>(); // Lưu nhiều ảnh
        private List<Student> _students;
        private readonly IStudentService studentService;
        public ParentMedicationRequest()
        {
            InitializeComponent();
            _medicationRequestService = new MedicationRequestService();
            studentService = new StudentService();
            dateSend.DisplayDateStart = DateTime.Now; // Chỉ cho chọn từ ngày mai trở đi
            dateSend.SelectedDate = DateTime.Now;     // Gợi ý mặc định là ngày mai
            dateSend.SelectedDateChanged += dateSend_SelectedDateChanged;           
        }

        private void LoadMedicationRequests()
        {
            var currentUser = (User)App.Current.Properties["CurrentUser"];
            var medicationService = new MedicationRequestService();
           
            var students = studentService.GetStudentsByParentId(currentUser.Id);
            var studentIds = students.Select(s => s.Id).ToHashSet();
            var allRequests = medicationService.GetMedicationRequests();
            var requests = allRequests.Where(r => r.StudentId != null && studentIds.Contains(r.StudentId.Value)).ToList();
            // Gán StudentName và StatusText cho từng request để binding DataGrid
            foreach (var req in requests)
            {
                req.StudentName = students.FirstOrDefault(s => s.Id == req.StudentId)?.FullName ?? "";
                req.StatusText = req.Status switch
                {
                    RequestStatus.Pending => "Đang chờ",
                    RequestStatus.Received => "Đã duyệt",
                    RequestStatus.Administered => "Đã cho uong",
                    RequestStatus.Returned => "Đã trả lại",
                };
            }
            if (requests == null || requests.Count == 0)
            {
                txtEmptyList.Visibility = Visibility.Visible;
                dgMedicationList.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtEmptyList.Visibility = Visibility.Collapsed;
                dgMedicationList.Visibility = Visibility.Visible;
                dgMedicationList.ItemsSource = requests;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var currentUser = (User)App.Current.Properties["CurrentUser"];
            var studentService = new StudentService();
            _students = studentService.GetStudentsByParentId(currentUser.Id);
            cboStudent.ItemsSource = _students;
            cboStudent.DisplayMemberPath = "FullName";
            cboStudent.SelectedValuePath = "Id";
            LoadMedicationRequests();
        }

        private void tabListMedicationReq_GotFocus(object sender, RoutedEventArgs e)
        {
            LoadMedicationRequests();
        }



        // Upload nhiều ảnh và lưu vào _imageBase64List
        //private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        //{
        //    var openFileDialog = new Microsoft.Win32.OpenFileDialog();
        //    openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
        //    openFileDialog.Multiselect = true;
        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        _imageBase64List.Clear();
        //        imgPreview.Source = null;
        //        imgPreview.Visibility = Visibility.Collapsed;
        //        foreach (var filePath in openFileDialog.FileNames)
        //        {
        //            try
        //            {
        //                byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
        //                string base64 = Convert.ToBase64String(imageBytes);
        //                _imageBase64List.Add(base64);
        //                // Hiển thị ảnh đầu tiên lên Image control
        //                if (_imageBase64List.Count == 1)
        //                {
        //                    using (var ms = new System.IO.MemoryStream(imageBytes))
        //                    {
        //                        var bitmap = new BitmapImage();
        //                        bitmap.BeginInit();
        //                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        //                        bitmap.StreamSource = ms;
        //                        bitmap.EndInit();
        //                        imgPreview.Source = bitmap;
        //                        imgPreview.Visibility = Visibility.Visible;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show($"Lỗi khi đọc ảnh: {ex.Message}");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        imgPreview.Source = null;
        //        imgPreview.Visibility = Visibility.Collapsed;
        //    }
        //}
        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Chọn hình ảnh hóa đơn thuốc";

            if (openFileDialog.ShowDialog() == true)
            {
                _imageBase64List.Clear();
                imgPreview.Source = null;
                imgPreview.Visibility = Visibility.Collapsed;

                foreach (var filePath in openFileDialog.FileNames)
                {
                    try
                    {
                        byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);

                        // Kiểm tra kích thước file (giới hạn 5MB)
                        if (imageBytes.Length > 5 * 1024 * 1024)
                        {
                            MessageBox.Show($"File {System.IO.Path.GetFileName(filePath)} quá lớn (>5MB). Vui lòng chọn file khác.",
                                          "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue;
                        }

                        string base64 = Convert.ToBase64String(imageBytes);
                        _imageBase64List.Add(base64);

                        // Hiển thị ảnh đầu tiên trong preview
                        if (_imageBase64List.Count == 1)
                        {
                            // Sử dụng BitmapImage trực tiếp từ file path thay vì Base64
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(filePath);
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();

                            imgPreview.Source = bitmap;
                            imgPreview.Visibility = Visibility.Visible;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi đọc ảnh {System.IO.Path.GetFileName(filePath)}: {ex.Message}",
                                      "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Cập nhật UI
                if (_imageBase64List.Count > 0)
                {
                    btnSelectImage.Content = $"Đã chọn {_imageBase64List.Count} ảnh";
                    if (imgPreview != null) // Kiểm tra null nếu button này tồn tại
                        imgPreview.IsEnabled = true;
                }
                else
                {
                    btnSelectImage.Content = "Tải lên hóa đơn thuốc";
                    if (imgPreview != null)
                        imgPreview.IsEnabled = false;
                }
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

        // Khi gửi request, lưu mảng ảnh
        private void btnSendRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Your validation code remains the same...

                // Create MedicationRequest with explicit UTC conversion
                MedicationRequest medicationRequest = new MedicationRequest
                {
                    Id = Guid.NewGuid(),
                    StudentId = Guid.Parse(cboStudent.SelectedValue.ToString()),
                    MedicationName = txtMedicationName.Text.Trim(),
                    Dosage = int.Parse(txtDosage.Text.Trim()),
                    NumberOfDayToTake = int.Parse(txtQuantity.Text.Trim()),
                    StartDate = dateSend.SelectedDate?.ToUniversalTime(), // Convert to UTC
                    EndDate = dateSend.SelectedDate?.AddDays(int.Parse(txtQuantity.Text.Trim()) - 1).ToUniversalTime(), // Convert to UTC
                    Instructions = txtNote.Text?.Trim(),
                    Status = RequestStatus.Pending,
                    //ImagesMedicalInvoice = new List<string>(), // Initialize as empty list
                    ImagesMedicalInvoice = _imageBase64List.ToList(),
                    CreatedBy = null,
                    UpdatedBy = null,
                    CreateAt = DateTime.UtcNow, // Use UTC
                    UpdateAt = DateTime.UtcNow  // Use UTC
                };

                _medicationRequestService.CreateMedicationRequest(medicationRequest);
                MessageBox.Show("Gửi yêu cầu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();

                // Load updated medication requests
                LoadMedicationRequests();

                // Switch to the "Danh sách thuốc đã gửi" tab
                tabListMedicationReq.IsSelected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            cboStudent.SelectedIndex = -1;
            txtMedicationName.Text = "";
            txtDosage.Text = "";
            txtQuantity.Text = "";
            dateSend.SelectedDate = DateTime.Now;
            txtNote.Text = "";
            _imageBase64List.Clear();
            imgPreview.Source = null;
            imgPreview.Visibility = Visibility.Collapsed;
        }
        private void BtnViewDetail_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var request = btn?.Tag as MedicationRequest;
            if (request != null)
            {
                var detailWindow = new DetailMedicationRequestWindow(request);
                detailWindow.ShowDialog();
            }
        }
    }
}
