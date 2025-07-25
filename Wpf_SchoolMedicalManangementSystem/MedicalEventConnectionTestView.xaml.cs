using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class MedicalEventConnectionTestView : Page
    {
        public ObservableCollection<string> TestMessages { get; set; }

        public MedicalEventConnectionTestView()
        {
            InitializeComponent();
            DataContext = this;

            TestMessages = new ObservableCollection<string>
            {
                "🟡 Đang kiểm tra kết nối cơ sở dữ liệu...",
                "⏳ Vui lòng chờ giây lát..."
            };

            _ = TestDatabaseConnection();
        }

        private async Task TestDatabaseConnection()
        {
            try
            {
                await Task.Delay(1000); // Simulate loading

                TestMessages.Add("🔍 Đang kiểm tra DbContext...");
                await Task.Delay(500);

                // Test basic connection
                using (var context = new DataAccessLayer.SwpSchoolMedicalManagementSystemContext())
                {
                    TestMessages.Add("✅ DbContext khởi tạo thành công");
                    await Task.Delay(500);

                    // Test table access
                    TestMessages.Add("🔍 Đang kiểm tra bảng MedicalIncidents...");
                    await Task.Delay(500);

                    var count = await context.MedicalIncidents.CountAsync();
                    TestMessages.Add($"✅ Kết nối bảng MedicalIncidents thành công. Tìm thấy {count} bản ghi.");

                    await Task.Delay(500);
                    TestMessages.Add("🔍 Đang thử truy vấn dữ liệu mẫu...");
                    await Task.Delay(500);

                    // Simple query without includes
                    var incidents = await context.MedicalIncidents.Take(3).ToListAsync();
                    TestMessages.Add($"✅ Truy vấn dữ liệu thành công. Lấy được {incidents.Count} bản ghi mẫu.");

                    foreach (var incident in incidents)
                    {
                        TestMessages.Add($"📋 ID: {incident.Id}, Ngày: {incident.IncidentDate:dd/MM/yyyy}, Loại: {incident.IncidentType}");
                    }

                    TestMessages.Add("🎉 Tất cả kiểm tra đều thành công!");
                    TestMessages.Add("✨ Hệ thống sẵn sàng hoạt động!");
                }
            }
            catch (Exception ex)
            {
                TestMessages.Add($"❌ Lỗi: {ex.Message}");
                if (ex.InnerException != null)
                {
                    TestMessages.Add($"🔍 Chi tiết: {ex.InnerException.Message}");
                }
                TestMessages.Add("💡 Vui lòng kiểm tra connection string và cơ sở dữ liệu.");
            }
        }

        private void BtnRetry_Click(object sender, RoutedEventArgs e)
        {
            TestMessages.Clear();
            TestMessages.Add("🔄 Đang thử lại kết nối...");
            _ = TestDatabaseConnection();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            // Return to dashboard
            MessageBox.Show("Quay lại Dashboard", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
