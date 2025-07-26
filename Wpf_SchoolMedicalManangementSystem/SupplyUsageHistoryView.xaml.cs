using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class SupplyUsageHistoryView : Window
    {
        private MedicalSupply _supply;
        private List<MedicalSupplyUsage> _usageHistory = new List<MedicalSupplyUsage>();

        public SupplyUsageHistoryView(MedicalSupply supply)
        {
            InitializeComponent();
            _supply = supply;
            LoadSupplyInfo();
            LoadUsageHistory();
        }

        private void LoadSupplyInfo()
        {
            txtSupplyName.Text = _supply.SupplyName;
            txtUnit.Text = _supply.Unit;
            txtCurrentStock.Text = _supply.Quantity?.ToString() ?? "0";
        }

        private void LoadUsageHistory()
        {
            try
            {
                // In a real application, this would load from the repository
                // For now, we'll use the navigation property or create sample data
                _usageHistory = _supply.MedicalSupplyUsages?.ToList() ?? new List<MedicalSupplyUsage>();

                ApplyDateFilter();
                UpdateSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải lịch sử sử dụng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyDateFilter()
        {
            var filteredHistory = _usageHistory.AsEnumerable();

            if (dpFromDate?.SelectedDate.HasValue == true)
            {
                filteredHistory = filteredHistory.Where(u => u.UsageDate >= dpFromDate.SelectedDate.Value);
            }

            if (dpToDate?.SelectedDate.HasValue == true)
            {
                filteredHistory = filteredHistory.Where(u => u.UsageDate <= dpToDate.SelectedDate.Value.AddDays(1));
            }

            if (dgUsageHistory != null)
            {
                dgUsageHistory.ItemsSource = filteredHistory.OrderByDescending(u => u.UsageDate).ToList();
            }
        }

        private void UpdateSummary()
        {
            var totalUsed = _usageHistory.Sum(u => u.QuantityUsed);
            var totalTransactions = _usageHistory.Count;

            txtTotalIn.Text = "N/A"; // Not applicable for usage records
            txtTotalOut.Text = totalUsed.ToString();
            txtTotalTransactions.Text = totalTransactions.ToString();
        }

        private void DateFilter_Changed(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgUsageHistory != null)
            {
                ApplyDateFilter();
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUsageHistory();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
