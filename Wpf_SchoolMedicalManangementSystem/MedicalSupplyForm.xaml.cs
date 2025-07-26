using System;
using System.Windows;
using BusinessObjects;
using DataAccessLayer;
using Microsoft.Win32;

namespace Wpf_SchoolMedicalManangementSystem
{
    /// <summary>
    /// Interaction logic for MedicalSupplyForm.xaml
    /// </summary>
    public partial class MedicalSupplyForm : Window
    {
        private readonly MedicalSupplyDAO _medicalSupplyDAO = new();
        private MedicalSupply? _editingSupply;

        public MedicalSupplyForm()
        {
            InitializeComponent();
        }

        public MedicalSupplyForm(MedicalSupply supply) : this()
        {
            _editingSupply = supply;
            HeaderText.Text = "Sửa vật tư";
            LoadSupplyData();
        }

        private void LoadSupplyData()
        {
            if (_editingSupply == null) return;

            SupplyNameTextBox.Text = _editingSupply.SupplyName;
            SupplyTypeComboBox.SelectedIndex = _editingSupply.SupplyType;
            QuantityTextBox.Text = _editingSupply.Quantity?.ToString();
            UnitTextBox.Text = _editingSupply.Unit;
            SupplierTextBox.Text = _editingSupply.Supplier;
            // Hiển thị ảnh đầu tiên nếu có
            ImageTextBox.Text = _editingSupply.Image != null && _editingSupply.Image.Length > 0 ? _editingSupply.Image[0] : string.Empty;
        }

        private void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Chọn hình ảnh",
                Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImageTextBox.Text = openFileDialog.FileName;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                var supply = _editingSupply ?? new MedicalSupply
                {
                    Id = Guid.NewGuid(),
                    CreateAt = DateTime.Now
                };

                supply.SupplyName = SupplyNameTextBox.Text.Trim();
                supply.SupplyType = SupplyTypeComboBox.SelectedIndex;
                supply.Quantity = int.TryParse(QuantityTextBox.Text, out var quantity) ? quantity : 0;
                supply.Unit = UnitTextBox.Text.Trim();
                supply.Supplier = SupplierTextBox.Text.Trim();
                // Lưu ảnh dưới dạng mảng (chỉ 1 ảnh, có thể mở rộng nếu cần)
                supply.Image = string.IsNullOrWhiteSpace(ImageTextBox.Text) ? Array.Empty<string>() : new string[] { ImageTextBox.Text.Trim() };
                supply.UpdateAt = DateTime.Now;

                if (_editingSupply == null)
                {
                    // Create new supply
                    _medicalSupplyDAO.CreateMedicalSupply(supply);
                }
                else
                {
                    // Update existing supply
                    _medicalSupplyDAO.UpdateMedicalSuplly(supply);
                }

                MessageBox.Show(_editingSupply == null ? "Tạo vật tư thành công!" : "Cập nhật vật tư thành công!",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(SupplyNameTextBox.Text))
            {
                MessageBox.Show("Tên vật tư không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                SupplyNameTextBox.Focus();
                return false;
            }

            if (SupplyTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn loại vật tư.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                SupplyTypeComboBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(QuantityTextBox.Text))
            {
                MessageBox.Show("Số lượng không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                QuantityTextBox.Focus();
                return false;
            }

            if (!int.TryParse(QuantityTextBox.Text, out var quantity) || quantity < 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên không âm.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                QuantityTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(UnitTextBox.Text))
            {
                MessageBox.Show("Đơn vị không được để trống.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                UnitTextBox.Focus();
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