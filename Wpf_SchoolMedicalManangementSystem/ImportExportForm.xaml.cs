using System;
using System.Windows;
using BusinessObjects;
using DataAccessLayer;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for ImportExportForm.xaml
    /// </summary>
    public partial class ImportExportForm : Window
    {
        private readonly MedicalSupplyDAO _medicalSupplyDAO = new();
        private MedicalSupply _supply;

        public ImportExportForm(MedicalSupply supply)
        {
            InitializeComponent();
            _supply = supply;
            LoadSupplyInfo();
        }

        private void LoadSupplyInfo()
        {
            SupplyNameText.Text = _supply.SupplyName ?? "N/A";
            CurrentQuantityText.Text = _supply.Quantity?.ToString() ?? "0";
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                var operationType = OperationTypeComboBox.SelectedIndex; // 0 = Import, 1 = Export
                var quantity = int.Parse(QuantityTextBox.Text);
                var reason = ReasonTextBox.Text.Trim();
                var performedBy = PerformedByTextBox.Text.Trim();

                // Update supply quantity
                if (operationType == 0) // Import
                {
                    _supply.Quantity = (_supply.Quantity ?? 0) + quantity;
                }
                else // Export
                {
                    if ((_supply.Quantity ?? 0) < quantity)
                    {
                        MessageBox.Show("Số lượng xuất vượt quá số lượng hiện có trong kho.", 
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    _supply.Quantity = (_supply.Quantity ?? 0) - quantity;
                }

                _supply.UpdateAt = DateTime.Now;
                _medicalSupplyDAO.UpdateMedicalSuplly(_supply);

                // In a real application, you would also log the import/export operation
                var operation = operationType == 0 ? "nhập" : "xuất";
                MessageBox.Show($"Đã {operation} {quantity} {_supply.Unit} thành công!", 
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thực hiện: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateForm()
        {
            if (OperationTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn loại thao tác.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                OperationTypeComboBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(QuantityTextBox.Text))
            {
                MessageBox.Show("Số lượng không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                QuantityTextBox.Focus();
                return false;
            }

            if (!int.TryParse(QuantityTextBox.Text, out var quantity) || quantity <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                QuantityTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(ReasonTextBox.Text))
            {
                MessageBox.Show("Lý do không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                ReasonTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(PerformedByTextBox.Text))
            {
                MessageBox.Show("Người thực hiện không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                PerformedByTextBox.Focus();
                return false;
            }

            return true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn hủy? Tất cả thay đổi sẽ bị mất.", 
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                this.DialogResult = false;
                this.Close();
            }
        }
    }
} 