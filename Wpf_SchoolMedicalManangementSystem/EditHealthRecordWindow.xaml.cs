using System.Windows.Controls;
using System.Windows;
using Services;
using BusinessObjects;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class EditHealthRecordDialog : Window
    {
        private readonly IHealthRecordService _healthRecordService = new HealthRecordService();
        private HealthRecord _healthRecord;
        public EditHealthRecordDialog(HealthRecord healthRecord)
        {
            InitializeComponent();
            _healthRecord = healthRecord;
            this.DataContext = healthRecord;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Gọi service update
            var result = _healthRecordService.UpdateHealthRecordAsync(_healthRecord);
            if (result)
            {
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
} 