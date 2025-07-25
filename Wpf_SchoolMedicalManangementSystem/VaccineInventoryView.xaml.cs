using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BusinessObjects;
using DataAccessLayer;
using SchoolMedicalManagementSystem.Enum;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for VaccineInventoryView.xaml
    /// </summary>
    public partial class VaccineInventoryView : Window
    {
        private readonly MedicalSupplyDAO _medicalSupplyDAO = new();
        public ObservableCollection<MedicalSupply> Supplies { get; set; } = new();
        public ObservableCollection<MedicalSupply> FilteredSupplies { get; set; } = new();

        public VaccineInventoryView()
        {
            InitializeComponent();
            InventoryDataGrid.ItemsSource = FilteredSupplies;
            LoadSupplies();
            UpdateStatistics();
        }

        private void LoadSupplies()
        {
            Supplies.Clear();
            foreach (var supply in _medicalSupplyDAO.GetMedicalSupplies())
            {
                Supplies.Add(supply);
            }
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            FilteredSupplies.Clear();
            var filtered = Supplies.AsEnumerable();

            // Apply search filter
            if (!string.IsNullOrEmpty(SearchBox.Text) && SearchBox.Text != "Tìm kiếm theo tên, nhà sản xuất...")
            {
                filtered = filtered.Where(s => s.SupplyName?.Contains(SearchBox.Text, StringComparison.OrdinalIgnoreCase) == true ||
                                             s.Supplier?.Contains(SearchBox.Text, StringComparison.OrdinalIgnoreCase) == true);
            }

            // Apply type filter
            if (TypeFilter != null && TypeFilter.SelectedIndex > 0)
            {
                var selectedType = TypeFilter.SelectedIndex - 1; // 0 = Medicine, 1 = MedicalEquipment, 2 = FirstAid, 3 = Other
                filtered = filtered.Where(s => s.SupplyType == selectedType);
            }

            // Apply status filter
            if (StatusFilter != null && StatusFilter.SelectedIndex > 0)
            {
                filtered = StatusFilter.SelectedIndex switch
                {
                    1 => filtered.Where(s => s.Quantity != null && s.Quantity > 10), // Còn nhiều
                    2 => filtered.Where(s => s.Quantity != null && s.Quantity > 0 && s.Quantity <= 10), // Sắp hết
                    3 => filtered.Where(s => s.Quantity == 0 || s.Quantity == null), // Hết hàng
                    _ => filtered
                };
            }

            foreach (var supply in filtered)
            {
                FilteredSupplies.Add(supply);
            }
        }

        private void UpdateStatistics()
        {
            var totalSupplies = Supplies.Count;
            var inStockSupplies = Supplies.Count(s => s.Quantity > 10);
            var lowStockSupplies = Supplies.Count(s => s.Quantity > 0 && s.Quantity <= 10);
            var outOfStockSupplies = Supplies.Count(s => s.Quantity == 0);
            var expiringSoonSupplies = Supplies.Count(s => s.Quantity > 0 && s.Quantity <= 5); // Mock logic

            TotalSuppliesText.Text = totalSupplies.ToString();
            InStockText.Text = inStockSupplies.ToString();
            LowStockText.Text = lowStockSupplies.ToString();
            OutOfStockText.Text = outOfStockSupplies.ToString();
            ExpiringSoonText.Text = expiringSoonSupplies.ToString();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadSupplies();
            UpdateStatistics();
        }

        private void AddSupply_Click(object sender, RoutedEventArgs e)
        {
            var supplyForm = new MedicalSupplyForm();
            if (supplyForm.ShowDialog() == true)
            {
                LoadSupplies();
                UpdateStatistics();
            }
        }

        private void EditSupply_Click(object sender, RoutedEventArgs e)
        {
            if (InventoryDataGrid.SelectedItem is MedicalSupply selected)
            {
                var supplyForm = new MedicalSupplyForm(selected);
                if (supplyForm.ShowDialog() == true)
                {
                    LoadSupplies();
                    UpdateStatistics();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một vật tư để sửa.", 
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ImportExport_Click(object sender, RoutedEventArgs e)
        {
            if (InventoryDataGrid.SelectedItem is MedicalSupply selected)
            {
                var importExportForm = new ImportExportForm(selected);
                if (importExportForm.ShowDialog() == true)
                {
                    LoadSupplies();
                    UpdateStatistics();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một vật tư để nhập/xuất.", 
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteSupply_Click(object sender, RoutedEventArgs e)
        {
            if (InventoryDataGrid.SelectedItem is MedicalSupply selected)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa vật tư '{selected.SupplyName}'?",
                    "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _medicalSupplyDAO.DeleteMedicalSupply(selected);
                        LoadSupplies();
                        UpdateStatistics();

                        MessageBox.Show("Đã xóa vật tư thành công!", 
                            "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa vật tư: {ex.Message}", 
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một vật tư để xóa.", 
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void Filter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
    }

    // Extension class to add display properties to MedicalSupply
    public static class MedicalSupplyExtensions
    {
        public static string TypeDisplay(this MedicalSupply supply)
        {
            return supply.SupplyType switch
            {
                0 => "Thuốc",
                1 => "Thiết bị y tế",
                2 => "Vật tư sơ cứu",
                3 => "Khác",
                _ => "Không xác định"
            };
        }

        public static string StatusDisplay(this MedicalSupply supply)
        {
            return supply.Quantity switch
            {
                0 => "Hết hàng",
                var q when q > 0 && q <= 5 => "Sắp hết",
                var q when q > 5 && q <= 10 => "Sắp hết",
                _ => "Còn hàng"
            };
        }
    }
}
