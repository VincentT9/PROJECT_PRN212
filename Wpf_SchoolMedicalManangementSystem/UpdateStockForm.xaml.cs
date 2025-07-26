using BusinessObjects;
using System;
using System.Windows;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class UpdateStockForm : Window
    {
        public MedicalSupply Supply { get; set; }
        public int UpdateQuantity { get; private set; }
        public string UpdateType { get; private set; }
        public string Reason { get; private set; }
        public string Notes { get; private set; }

        public UpdateStockForm(MedicalSupply supply)
        {
            InitializeComponent();
            Supply = supply;
            LoadSupplyData();
        }

        private void LoadSupplyData()
        {
            txtSupplyName.Text = Supply.SupplyName;
            txtUnit.Text = Supply.Unit;
            txtCurrentStock.Text = Supply.Quantity?.ToString() ?? "0";
            cmbUpdateType.SelectedIndex = 0; // Default to "In"
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                var selectedItem = (System.Windows.Controls.ComboBoxItem)cmbUpdateType.SelectedItem;
                UpdateType = selectedItem.Tag.ToString();
                UpdateQuantity = int.Parse(txtQuantity.Text);
                Reason = txtReason.Text;
                Notes = txtNotes.Text;

                DialogResult = true;
                Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateInput()
        {
            if (cmbUpdateType.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn loại cập nhật.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtQuantity.Text) || !int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ (lớn hơn 0).", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtReason.Text))
            {
                MessageBox.Show("Vui lòng nhập lý do cập nhật.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
