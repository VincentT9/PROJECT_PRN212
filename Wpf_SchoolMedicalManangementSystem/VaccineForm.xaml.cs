using System;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class VaccineForm : Window
    {
        private MedicalSupply? _vaccine;
        private bool _isEditMode;

        public VaccineForm()
        {
            InitializeComponent();
            _isEditMode = false;
            HeaderText.Text = "Thêm vaccine mới";
        }

        public VaccineForm(MedicalSupply vaccine)
        {
            InitializeComponent();
            _vaccine = vaccine;
            _isEditMode = true;
            HeaderText.Text = "Sửa vaccine";
            LoadVaccineData();
        }

        private void LoadVaccineData()
        {
            if (_vaccine != null)
            {
                VaccineNameTextBox.Text = _vaccine.SupplyName ?? "";
                QuantityTextBox.Text = _vaccine.Quantity?.ToString() ?? "0";
                UnitTextBox.Text = _vaccine.Unit ?? "";
                SupplierTextBox.Text = _vaccine.Supplier ?? "";
                
                // Note: MedicalSupply doesn't have ExpiryDate, so we'll skip that
                // You might want to add this property to the MedicalSupply class

                // Set vaccine type based on supply type
                VaccineTypeComboBox.SelectedIndex = _vaccine.SupplyType;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(VaccineNameTextBox.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên vaccine!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(QuantityTextBox.Text) || !int.TryParse(QuantityTextBox.Text, out int quantity))
                {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(UnitTextBox.Text))
                {
                    MessageBox.Show("Vui lòng nhập đơn vị!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Create or update vaccine
                if (_vaccine == null)
                {
                    _vaccine = new MedicalSupply();
                }

                _vaccine.SupplyName = VaccineNameTextBox.Text;
                _vaccine.Quantity = quantity;
                _vaccine.Unit = UnitTextBox.Text;
                _vaccine.Supplier = SupplierTextBox.Text;
                _vaccine.SupplyType = VaccineTypeComboBox.SelectedIndex;
                _vaccine.CreateAt = DateTime.Now;
                _vaccine.UpdateAt = DateTime.Now;

                // Get selected vaccine type
                if (VaccineTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    string vaccineType = selectedItem.Content.ToString();
                    // You can add additional logic here based on vaccine type
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu vaccine: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public MedicalSupply? GetVaccine()
        {
            return _vaccine;
        }
    }
}
