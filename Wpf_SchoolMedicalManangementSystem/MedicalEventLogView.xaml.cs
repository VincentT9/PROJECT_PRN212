using BusinessObjects;
using Repositories;
using Services;
using SchoolMedicalManagementSystem.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class MedicalEventLogView : Page, INotifyPropertyChanged
    {
        private readonly MedicalIncidentService _medicalIncidentService;
        private ObservableCollection<MedicalIncident>? _medicalIncidents;
        private string? _searchTerm;
        private int? _selectedIncidentType;
        private int? _selectedStatus;
        private DateTime? _fromDate;
        private DateTime? _toDate;
        MedicalSupplyUsageService _medicalSupplyUsageService = new MedicalSupplyUsageService(new MedicalSupplyUsageRepository());
        private TextBlock? TxtStatus { get { return (TextBlock?)this.FindName("txtStatus"); } }
        private TextBlock? TxtRecordCount { get { return (TextBlock?)this.FindName("txtRecordCount"); } }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<MedicalIncident> MedicalIncidents
        {
            get => _medicalIncidents ?? new ObservableCollection<MedicalIncident>();
            set
            {
                _medicalIncidents = value;
                OnPropertyChanged(nameof(MedicalIncidents));
                UpdateRecordCount();
            }
        }

        public string SearchTerm
        {
            get => _searchTerm ?? string.Empty;
            set
            {
                _searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));
            }
        }

        public int? SelectedIncidentType
        {
            get => _selectedIncidentType;
            set
            {
                _selectedIncidentType = value;
                OnPropertyChanged(nameof(SelectedIncidentType));
            }
        }

        public int? SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }

        public DateTime? FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
                OnPropertyChanged(nameof(FromDate));
            }
        }

        public DateTime? ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
                OnPropertyChanged(nameof(ToDate));
            }
        }

        public MedicalEventLogView()
        {
            InitializeComponent();
            DataContext = this;

            try
            {
                var repository = new MedicalIncidentRepository();
                _medicalIncidentService = new MedicalIncidentService(repository);
                MedicalIncidents = new ObservableCollection<MedicalIncident>();
                InitializeComboBoxes();
                // KHÔNG load dữ liệu ở đây, sẽ load ở Page_Loaded
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                // Khởi tạo service mặc định để tránh null reference
                var repository = new MedicalIncidentRepository();
                _medicalIncidentService = new MedicalIncidentService(repository);
                MedicalIncidents = new ObservableCollection<MedicalIncident>();
            }
        }

        private void InitializeComboBoxes()
        {
            // Initialize Incident Type ComboBox
            var incidentTypes = new List<KeyValuePair<int?, string>>
            {
                new KeyValuePair<int?, string>(null, "Tất cả"),
                new KeyValuePair<int?, string>((int)IncidentType.Accident, "Tai nạn"),
                new KeyValuePair<int?, string>((int)IncidentType.Fever, "Sốt"),
                new KeyValuePair<int?, string>((int)IncidentType.Fall, "Ngã"),
                new KeyValuePair<int?, string>((int)IncidentType.Epidemic, "Dịch bệnh"),
                new KeyValuePair<int?, string>((int)IncidentType.Other, "Khác")
            };
            cmbIncidentType.ItemsSource = incidentTypes;
            cmbIncidentType.SelectedIndex = 0;

            // Initialize Status ComboBox
            var statuses = new List<KeyValuePair<int?, string>>
            {
                new KeyValuePair<int?, string>(null, "Tất cả"),
                new KeyValuePair<int?, string>((int)IncidentStatus.Reported, "Đã báo cáo"),
                new KeyValuePair<int?, string>((int)IncidentStatus.Processing, "Đang xử lý"),
                new KeyValuePair<int?, string>((int)IncidentStatus.Resolved, "Đã giải quyết")
            };
            cmbStatus.ItemsSource = statuses;
            cmbStatus.SelectedIndex = 0;
        }

        private async Task LoadMedicalIncidents()
        {
            try
            {
                if (TxtStatus != null) TxtStatus.Text = "Đang tải dữ liệu...";

                var incidents = await _medicalIncidentService.GetAllMedicalIncidentsAsync();

                var refreshedList = new ObservableCollection<MedicalIncident>();
                foreach (var incident in incidents)
                {
                    // Gán giá trị cho các thuộc tính Display
                    incident.IncidentTypeDisplay = GetIncidentTypeDisplay(incident.IncidentType);
                    incident.DescriptionDisplay = string.IsNullOrWhiteSpace(incident.Description) ? "" : incident.Description;
                    incident.ActionsTakenDisplay = string.IsNullOrWhiteSpace(incident.ActionsTaken) ? "Chưa có dữ liệu" : incident.ActionsTaken;
                    incident.OutcomeDisplay = string.IsNullOrWhiteSpace(incident.Outcome) ? "Chưa có dữ liệu" : incident.Outcome;
                    incident.StatusDisplay = GetStatusDisplay(incident.Status);
                    refreshedList.Add(incident);
                }
                MedicalIncidents = refreshedList;
                if (dgMedicalIncidents != null) dgMedicalIncidents.Items.Refresh();
                if (TxtStatus != null) TxtStatus.Text = "Tải dữ liệu thành công";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: LoadMedicalIncidents {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                if (TxtStatus != null) TxtStatus.Text = "Lỗi tải dữ liệu";
            }
        }

        private string GetIncidentTypeDisplay(int incidentType)
        {
            return ((IncidentType)incidentType) switch
            {
                IncidentType.Accident => "Tai nạn",
                IncidentType.Fever => "Sốt",
                IncidentType.Fall => "Ngã",
                IncidentType.Epidemic => "Dịch bệnh",
                IncidentType.Other => "Khác",
                _ => "Không xác định"
            };
        }

        private string GetStatusDisplay(int status)
        {
            return ((IncidentStatus)status) switch
            {
                IncidentStatus.Reported => "Đã báo cáo",
                IncidentStatus.Processing => "Đang xử lý",
                IncidentStatus.Resolved => "Đã giải quyết",
                _ => "Không xác định"
            };
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            await PerformSearch();
        }

        private async void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            SearchTerm = string.Empty;
            SelectedIncidentType = null;
            SelectedStatus = null;
            FromDate = null;
            ToDate = null;

            cmbIncidentType.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;

            await LoadMedicalIncidents();
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addEditForm = new MedicalIncidentForm();
            if (addEditForm.ShowDialog() == true)
            {
                await LoadMedicalIncidents();
                DataContext = null;
                DataContext = this;
            }
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var incident = button?.Tag as MedicalIncident;

            if (incident != null)
            {
                var addEditForm = new MedicalIncidentForm(incident);
                if (addEditForm.ShowDialog() == true)
                {
                    await LoadMedicalIncidents();
                    DataContext = null;
                    DataContext = this;
                }
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var incident = button?.Tag as MedicalIncident;

            if (incident != null)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa sự kiện y tế này?\n\nHọc sinh: {incident.Student?.FullName}\nNgày: {incident.IncidentDate:dd/MM/yyyy}",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var success = await _medicalIncidentService.DeleteMedicalIncidentAsync(incident.Id);
                        if (success)
                        {
                            MessageBox.Show("Xóa sự kiện thành công!", "Thành công",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            await LoadMedicalIncidents();
                            DataContext = null;
                            DataContext = this;
                        }
                        else
                        {
                            MessageBox.Show("Không thể xóa sự kiện!", "Lỗi",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private async void DgMedicalIncidents_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedIncident = dgMedicalIncidents.SelectedItem as MedicalIncident;
            if (selectedIncident != null)
            {
                var detailForm = new MedicalIncidentDetailView(selectedIncident);
                detailForm.ShowDialog();
            }
        }

        private async Task PerformSearch()
        {
            try
            {
                if (TxtStatus != null) TxtStatus.Text = "Đang tìm kiếm...";

                List<MedicalIncident> results;

                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    results = await _medicalIncidentService.SearchMedicalIncidentsAsync(SearchTerm);
                }
                else
                {
                    results = await _medicalIncidentService.GetAllMedicalIncidentsAsync();
                }

                // Apply filters
                if (SelectedIncidentType.HasValue)
                {
                    results = results.Where(x => x.IncidentType == SelectedIncidentType.Value).ToList();
                }

                if (SelectedStatus.HasValue)
                {
                    results = results.Where(x => x.Status == SelectedStatus.Value).ToList();
                }

                if (FromDate.HasValue)
                {
                    results = results.Where(x => x.IncidentDate.Date >= FromDate.Value.Date).ToList();
                }

                if (ToDate.HasValue)
                {
                    results = results.Where(x => x.IncidentDate.Date <= ToDate.Value.Date).ToList();
                }

                var refreshedList = new ObservableCollection<MedicalIncident>();
                foreach (var incident in results)
                {
                    // Gán giá trị cho các thuộc tính Display khi tìm kiếm
                    incident.IncidentTypeDisplay = GetIncidentTypeDisplay(incident.IncidentType);
                    incident.DescriptionDisplay = string.IsNullOrWhiteSpace(incident.Description) ? "" : incident.Description;
                    incident.ActionsTakenDisplay = string.IsNullOrWhiteSpace(incident.ActionsTaken) ? "Chưa có dữ liệu" : incident.ActionsTaken;
                    incident.OutcomeDisplay = string.IsNullOrWhiteSpace(incident.Outcome) ? "Chưa có dữ liệu" : incident.Outcome;
                    incident.StatusDisplay = GetStatusDisplay(incident.Status);
                    refreshedList.Add(incident);
                }
                MedicalIncidents = refreshedList;
                if (dgMedicalIncidents != null) dgMedicalIncidents.Items.Refresh();
                if (TxtStatus != null) TxtStatus.Text = $"Tìm thấy {results.Count} kết quả";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                if (TxtStatus != null) TxtStatus.Text = "Lỗi tìm kiếm";
            }
        }

        private void UpdateRecordCount()
        {
            if (TxtRecordCount != null) TxtRecordCount.Text = $"Tổng số: {MedicalIncidents?.Count ?? 0} bản ghi";
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Event handler for Page Loaded
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadMedicalIncidents();
            DataContext = null;
            DataContext = this;
        }

        // Xem vật tư đã sử dụng
        private void BtnViewSupplies_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var incident = button?.Tag as MedicalIncident;
            var selectedIncident = dgMedicalIncidents.SelectedItem as MedicalIncident;
            if (selectedIncident == null)
            {

            }
            if (incident != null)
            {
                // Lấy danh sách vật tư sử dụng từ DAO
                var usages = _medicalSupplyUsageService.GetUsagesByIncidentId(incident.Id);

                var suppliesView = new MedicalIncidentSuppliesView(incident, usages);
                suppliesView.Owner = Window.GetWindow(this);
                suppliesView.ShowDialog();
            }
        }
    }
}