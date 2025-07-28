

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using Services;
using SchoolMedicalManagementSystem.Enum;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class MedicalSuppliesInventoryView : Page, INotifyPropertyChanged
    {
        public List<KeyValuePair<SupplyType, string>> SupplyTypeList { get; set; }

        private SupplyType? _selectedSupplyType;
        public SupplyType? SelectedSupplyType
        {
            get => _selectedSupplyType;
            set
            {
                _selectedSupplyType = value;
                OnPropertyChanged(nameof(SelectedSupplyType));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<MedicalSupply> _medicalSupplies = new ObservableCollection<MedicalSupply>();
        private string _searchTerm = string.Empty;
        private string _selectedStockStatus = string.Empty;



        public ObservableCollection<MedicalSupply> MedicalSupplies
        {
            get => _medicalSupplies;
            set
            {
                _medicalSupplies = value;
                OnPropertyChanged(nameof(MedicalSupplies));
            }
        }


        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));
            }
        }


        public string SelectedStockStatus
        {
            get => _selectedStockStatus;
            set
            {
                _selectedStockStatus = value;
                OnPropertyChanged(nameof(SelectedStockStatus));
            }
        }


        private MedicalSupply? _selectedMedicalSupply;
        public MedicalSupply? SelectedMedicalSupply
        {
            get => _selectedMedicalSupply;
            set
            {
                _selectedMedicalSupply = value;
                OnPropertyChanged(nameof(SelectedMedicalSupply));
            }
        }


        private readonly MedicalSupplyService _medicalSupplyService = new MedicalSupplyService();

        public MedicalSuppliesInventoryView()
        {
            InitializeComponent();
            DataContext = this;
            SupplyTypeList = new List<KeyValuePair<SupplyType, string>>
            {
                new KeyValuePair<SupplyType, string>(SupplyType.Medicine, "Thuốc"),
                new KeyValuePair<SupplyType, string>(SupplyType.MedicalEquipment, "Thiết bị y tế"),
                new KeyValuePair<SupplyType, string>(SupplyType.FirstAid, "Vật tư sơ cứu"),
                new KeyValuePair<SupplyType, string>(SupplyType.Other, "Khác")
            };
            LoadMedicalSupplies();
            UpdateRecordCount();
        }


        private void LoadMedicalSupplies()
        {
            try
            {
                var list = _medicalSupplyService.GetMedicalSupplies();
                MedicalSupplies = new ObservableCollection<MedicalSupply>(list);
                UpdateRecordCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải vật tư y tế: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var all = _medicalSupplyService.GetMedicalSupplies();
                var filtered = all;
                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    filtered = filtered.FindAll(s => s.SupplyName != null && s.SupplyName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
                }
                MedicalSupplies = new ObservableCollection<MedicalSupply>(filtered);
                UpdateRecordCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            SearchTerm = string.Empty;
            SelectedStockStatus = string.Empty;
            LoadMedicalSupplies();
            UpdateRecordCount();
        }



        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new MedicalSupplyForm();
            if (addWindow.ShowDialog() == true)
            {
                LoadMedicalSupplies();
                UpdateRecordCount();
            }
        }



        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedMedicalSupply == null)
            {
                MessageBox.Show("Vui lòng chọn vật tư để sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var editWindow = new MedicalSupplyForm(SelectedMedicalSupply);
            if (editWindow.ShowDialog() == true)
            {
                LoadMedicalSupplies();
                UpdateRecordCount();
            }
        }



        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedMedicalSupply == null)
            {
                MessageBox.Show("Vui lòng chọn vật tư để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show($"Bạn có chắc muốn xóa vật tư '{SelectedMedicalSupply.SupplyName}'?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _medicalSupplyService.DeleteMedicalSupply(SelectedMedicalSupply);
                    LoadMedicalSupplies();
                    UpdateRecordCount();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa vật tư: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void UpdateRecordCount()
        {
            if (Application.Current?.Dispatcher != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var txtRecordCount = this.FindName("txtRecordCount") as System.Windows.Controls.TextBlock;
                    if (txtRecordCount != null)
                    {
                        txtRecordCount.Text = $"Tổng số: {MedicalSupplies?.Count ?? 0} vật tư";
                    }
                });
            }
        }



        private void BtnUpdateStock_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedMedicalSupply == null)
            {
                MessageBox.Show("Vui lòng chọn vật tư để cập nhật tồn kho.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var updateWindow = new UpdateStockForm(SelectedMedicalSupply);
            if (updateWindow.ShowDialog() == true)
            {
                LoadMedicalSupplies();
            }
        }



        private void BtnViewUsageHistory_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedMedicalSupply == null)
            {
                MessageBox.Show("Vui lòng chọn vật tư để xem lịch sử sử dụng.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var historyWindow = new SupplyUsageHistoryView(SelectedMedicalSupply);
            historyWindow.ShowDialog();
        }


        private void DgMedicalSupplies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                SelectedMedicalSupply = e.AddedItems[0] as MedicalSupply;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
