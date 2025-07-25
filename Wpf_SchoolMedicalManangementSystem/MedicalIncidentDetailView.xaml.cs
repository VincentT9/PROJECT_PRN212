using BusinessObjects;
using SchoolMedicalManagementSystem.Enum;
using System.Windows;

namespace Wpf_SchoolMedicalManangementSystem
{
    public partial class MedicalIncidentDetailView : Window
    {
        private readonly MedicalIncident _incident;

        public MedicalIncidentDetailView(MedicalIncident incident)
        {
            InitializeComponent();
            _incident = incident;
            LoadIncidentDetails();
        }

        private void LoadIncidentDetails()
        {
            if (_incident == null) return;

            txtStudent.Text = _incident.Student?.FullName ?? "Không xác định";
            txtMedicalStaff.Text = _incident.MedicalStaff?.FullName ?? "Chưa phân công";
            txtIncidentType.Text = GetIncidentTypeDisplay(_incident.IncidentType);
            txtIncidentDate.Text = _incident.IncidentDate.ToString("dd/MM/yyyy HH:mm");
            txtStatus.Text = GetStatusDisplay(_incident.Status);
            txtDescription.Text = _incident.Description != null ? string.Join(", ", _incident.Description) : "";
            txtActionsTaken.Text = _incident.ActionsTaken != null ? string.Join(", ", _incident.ActionsTaken) : "";
            txtOutcome.Text = _incident.Outcome ?? "";
            txtCreatedBy.Text = _incident.CreatedBy ?? "";
            txtCreatedAt.Text = _incident.CreateAt.ToString("dd/MM/yyyy HH:mm");
            txtUpdatedBy.Text = _incident.UpdatedBy ?? "";
            txtUpdatedAt.Text = _incident.UpdateAt.ToString("dd/MM/yyyy HH:mm");
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

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var editForm = new MedicalIncidentForm(_incident);
            if (editForm.ShowDialog() == true)
            {
                // Refresh the parent window or close this detail view
                DialogResult = true;
                Close();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
