using BusinessObjects;

using Repositories;
using Services;
using SchoolMedicalManagementSystem.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class MedicalIncidentForm : Window
    {
        private readonly MedicalIncidentService _medicalIncidentService;
        private readonly MedicalSupplyUsageService _medicalSupplyUsageService;
        private readonly StudentService _studentService;
        private readonly UserService _userService;
        private readonly MedicalSupplyService _medicalSupplyService;
        private MedicalIncident _currentIncident;
        private bool _isEditMode;

        // Model for binding medical supplies selection
        public class MedicalSupplySelection
        {
            public Guid? IncidentId { get; set; } 
            public Guid? MedicalSupplyId { get; set; } // Added for linking to supply

            public int Quantity { get; set; }
            public DateTime UsageDate { get; set; } = DateTime.UtcNow;
            public string Notes { get; set; } = string.Empty;
        }

        private ObservableCollection<MedicalSupplySelection> _selectedSupplies = new ObservableCollection<MedicalSupplySelection>();
        private List<MedicalSupply> _allMedicalSupplies = new List<MedicalSupply>();

        public MedicalIncidentForm()
        {
            InitializeComponent();

            // Initialize services in constructor
            var medicalIncidentRepository = new MedicalIncidentRepository();
            _medicalIncidentService = new MedicalIncidentService(medicalIncidentRepository);
            var medicalSupplyUsageRepository = new MedicalSupplyUsageRepository();
            _medicalSupplyUsageService = new MedicalSupplyUsageService(medicalSupplyUsageRepository);

            var studentRepository = new StudentRepository();
            _studentService = new StudentService(studentRepository);

            var userRepository = new UserRepository();
            _userService = new UserService(userRepository);

            var medicalSupplyRepository = new MedicalSupplyRepository();
            _medicalSupplyService = new MedicalSupplyService(medicalSupplyRepository);

            _currentIncident = new MedicalIncident();
            InitializeForm();
            _isEditMode = false;
        }

        public MedicalIncidentForm(MedicalIncident incident)
        {
            InitializeComponent();

            // Initialize services in constructor
            var medicalIncidentRepository = new MedicalIncidentRepository();
            _medicalIncidentService = new MedicalIncidentService(medicalIncidentRepository);
            var medicalSupplyUsageRepository = new MedicalSupplyUsageRepository();
            _medicalSupplyUsageService = new MedicalSupplyUsageService(medicalSupplyUsageRepository);

            var studentRepository = new StudentRepository();
            _studentService = new StudentService(studentRepository);

            var userRepository = new UserRepository();
            _userService = new UserService(userRepository);

            var medicalSupplyRepository = new MedicalSupplyRepository();
            _medicalSupplyService = new MedicalSupplyService(medicalSupplyRepository);

            InitializeForm();
            _currentIncident = incident;
            _isEditMode = true;
            txtTitle.Text = "SỬA SỰ KIỆN Y TẾ";
            LoadIncidentData();
        }

        private async void InitializeForm()
        {
            InitializeComboBoxes();
            await LoadStudents();
            await LoadMedicalStaff();
            await LoadMedicalSupplies();
            InitializeTimeComboBoxes();

            if (!_isEditMode)
            {
                // Set default values for new incident
                dpIncidentDate.SelectedDate = DateTime.Now;
                cmbHour.SelectedValue = DateTime.Now.Hour;
                cmbMinute.SelectedValue = DateTime.Now.Minute;
                cmbStatus.SelectedValue = (int)IncidentStatus.Reported;
            }
        }

        private void InitializeComboBoxes()
        {
            // Initialize Incident Type ComboBox
            var incidentTypes = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>((int)IncidentType.Accident, "Tai nạn"),
                new KeyValuePair<int, string>((int)IncidentType.Fever, "Sốt"),
                new KeyValuePair<int, string>((int)IncidentType.Fall, "Ngã"),
                new KeyValuePair<int, string>((int)IncidentType.Epidemic, "Dịch bệnh"),
                new KeyValuePair<int, string>((int)IncidentType.Other, "Khác")
            };
            cmbIncidentType.ItemsSource = incidentTypes;

            // Initialize Status ComboBox
            var statuses = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>((int)IncidentStatus.Reported, "Đã báo cáo"),
                new KeyValuePair<int, string>((int)IncidentStatus.Processing, "Đang xử lý"),
                new KeyValuePair<int, string>((int)IncidentStatus.Resolved, "Đã giải quyết")
            };
            cmbStatus.ItemsSource = statuses;
        }

        private void InitializeTimeComboBoxes()
        {
            // Initialize Hour ComboBox (0-23)
            var hours = Enumerable.Range(0, 24).ToList();
            cmbHour.ItemsSource = hours;

            // Initialize Minute ComboBox (0, 15, 30, 45)
            var minutes = new List<int> { 0, 15, 30, 45 };
            cmbMinute.ItemsSource = minutes;
        }

        private async Task LoadStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                cmbStudent.ItemsSource = students;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách học sinh: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadMedicalStaff()
        {
            try
            {
                var medicalStaff = await _userService.GetUsersByRoleAsync(UserRole.MedicalStaff);
                cmbMedicalStaff.ItemsSource = medicalStaff;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách nhân viên y tế: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadMedicalSupplies()
        {
            try
            {
                _allMedicalSupplies = (await _medicalSupplyService.GetAllMedicalSuppliesAsync()).ToList();
                dgMedicalSupplies.ItemsSource = _selectedSupplies;
                // Set ItemsSource for ComboBox column
                var col = dgMedicalSupplies.Columns[0] as System.Windows.Controls.DataGridComboBoxColumn;
                if (col != null)
                {
                    col.ItemsSource = _allMedicalSupplies;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải vật tư y tế: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //// Đảm bảo luôn có ít nhất một dòng trống để chọn vật tư
            //if (_selectedSupplies.Count == 0)
            //{
            //    _selectedSupplies.Add(new MedicalSupplySelection());
            //}
        }

        private void LoadIncidentData()
        {
            if (_currentIncident == null) return;

            cmbStudent.SelectedValue = _currentIncident.StudentId;
            cmbMedicalStaff.SelectedValue = _currentIncident.MedicalStaffId;
            cmbIncidentType.SelectedValue = _currentIncident.IncidentType;
            dpIncidentDate.SelectedDate = _currentIncident.IncidentDate.Date;
            cmbHour.SelectedValue = _currentIncident.IncidentDate.Hour;
            cmbMinute.SelectedValue = _currentIncident.IncidentDate.Minute;


            txtDescription.Text = _currentIncident.Description ?? string.Empty;
            txtActionsTaken.Text = _currentIncident.ActionsTaken ?? string.Empty;

            txtOutcome.Text = _currentIncident.Outcome ?? "";

            cmbStatus.SelectedValue = _currentIncident.Status;

            // Load medical supplies used for this incident
            _selectedSupplies.Clear();
            if (_currentIncident.MedicalSupplyUsages != null && _currentIncident.MedicalSupplyUsages.Count > 0)
            {
                foreach (var usage in _currentIncident.MedicalSupplyUsages)
                {
                    _selectedSupplies.Add(new MedicalSupplySelection
                    {
                        MedicalSupplyId = usage.SupplyId,
                        Quantity = usage.QuantityUsed
                    });
                }
            }
            else
            {
                _selectedSupplies.Add(new MedicalSupplySelection());
            }
            dgMedicalSupplies.ItemsSource = _selectedSupplies;
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                var incident = CreateIncidentFromForm();
                var suppliesToUse = _selectedSupplies.Where(s => s.MedicalSupplyId.HasValue && s.Quantity > 0).ToList();
                // Check stock for each supply
                foreach (var supply in suppliesToUse)
                {
                    var supplyObj = _allMedicalSupplies.FirstOrDefault(x => x.Id == supply.MedicalSupplyId);
                    if (supplyObj == null)
                    {
                        MessageBox.Show($"Vật tư không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (supply.Quantity > supplyObj.Quantity)
                    {
                        MessageBox.Show($"Vật tư '{supplyObj.SupplyName}' chỉ còn {supplyObj.Quantity} trong kho!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                bool success;

                if (_isEditMode)
                {
                    incident.Id = _currentIncident.Id;
                    success = await _medicalIncidentService.UpdateMedicalIncidentAsync(incident);
                }
                else
                {
                    success = await _medicalIncidentService.CreateMedicalIncidentAsync(incident);
                }

                if (success)
                {

                    // Lưu thông tin vật tư đã dùng vào bảng liên kết
                    if (incident.MedicalSupplyUsages != null && incident.MedicalSupplyUsages.Count > 0)
                    {
                        await _medicalSupplyUsageService.AddMedicalSupplyUsagesAsync(incident.MedicalSupplyUsages.ToList());
                    }
              
                    // Trừ kho vật tư
                    foreach (var supply in suppliesToUse)
                    {
                        // Giảm số lượng vật tư trong kho
                        var supplyObj = _allMedicalSupplies.FirstOrDefault(x => x.Id == supply.MedicalSupplyId);
                        if (supplyObj != null)
                        {
                            supplyObj.Quantity -= supply.Quantity;
                            await _medicalSupplyService.UpdateMedicalSupplyAsync(supplyObj);
                        }
                    }
                    MessageBox.Show(
                    "Đã trừ xong",
                       "Thành công",
                       MessageBoxButton.OK,
                       MessageBoxImage.Information);
                    MessageBox.Show(
                        _isEditMode ? "Cập nhật sự kiện thành công!" : "Thêm sự kiện thành công!",
                        "Thành công",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    DialogResult = true;

                    Close();
                }
                else
                {
                    MessageBox.Show(
                        _isEditMode ? "Không thể cập nhật sự kiện!" : "Không thể thêm sự kiện!",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateForm()
        {
            if (cmbStudent.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn học sinh!", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbStudent.Focus();
                return false;
            }

            if (cmbIncidentType.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn loại sự kiện!", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbIncidentType.Focus();
                return false;
            }

            if (!dpIncidentDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng chọn ngày sự kiện!", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                dpIncidentDate.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Vui lòng nhập mô tả sự kiện!", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtDescription.Focus();
                return false;
            }

            if (cmbMedicalStaff.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên y tế!", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbMedicalStaff.Focus();
                return false;
            }
            return true;
        }

        private MedicalIncident CreateIncidentFromForm()
        {
            var selectedDate = dpIncidentDate.SelectedDate.Value;
            var hour = (int)(cmbHour.SelectedValue ?? 0);
            var minute = (int)(cmbMinute.SelectedValue ?? 0);
            var incidentDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, minute, 0, DateTimeKind.Local).ToUniversalTime();

            // Lấy dữ liệu text từ form
            string descriptionText = txtDescription.Text.Trim();
            string actionsTakenText = txtActionsTaken.Text.Trim();
            string outcomeText = txtOutcome.Text.Trim();


            // Tạo một incident mới với các thuộc tính cơ bản
            var incident = new MedicalIncident
            {
                Id = _isEditMode ? _currentIncident.Id : Guid.NewGuid(),
                StudentId = (Guid)cmbStudent.SelectedValue,
                MedicalStaffId = cmbMedicalStaff.SelectedValue as Guid?,
                IncidentType = (int)cmbIncidentType.SelectedValue,
                IncidentDate = incidentDateTime,
                Description = descriptionText,
                ActionsTaken = actionsTakenText,
                Outcome = outcomeText,
                Status = (int)cmbStatus.SelectedValue,
                CreatedBy = LoginWindow.CurrentUser?.FullName ?? "System",
                UpdatedBy = LoginWindow.CurrentUser?.FullName ?? "System",
                CreateAt = _isEditMode ? _currentIncident.CreateAt : DateTime.Now,
                UpdateAt = DateTime.Now
            };

            // Cập nhật vật tư sử dụng
            incident.MedicalSupplyUsages = new List<MedicalSupplyUsage>();
            foreach (var supply in _selectedSupplies)
            {
                if (supply.MedicalSupplyId.HasValue && supply.Quantity > 0)
                {
                    var supplyObj = _allMedicalSupplies.FirstOrDefault(x => x.Id == supply.MedicalSupplyId);
                    incident.MedicalSupplyUsages.Add(new MedicalSupplyUsage
                    {

                        IncidentId = incident.Id,
                        SupplyId = supplyObj.Id,
                        QuantityUsed = supply.Quantity,
                        UsageDate = incidentDateTime,
                        Notes = null,
                    });
                }
            }
            MessageBox.Show("Đã thêm vật tư y tế thành công! Xong hàm  CreateIncidentFromForm", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            return incident;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
