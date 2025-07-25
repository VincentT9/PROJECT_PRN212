using BusinessObjects;
using SchoolMedicalManagementSystem.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IMedicalIncidentService
    {
        Task<List<MedicalIncident>> GetAllMedicalIncidentsAsync();
        Task<MedicalIncident?> GetMedicalIncidentByIdAsync(Guid id);
        Task<List<MedicalIncident>> GetMedicalIncidentsByStudentIdAsync(Guid studentId);
        Task<List<MedicalIncident>> GetMedicalIncidentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<MedicalIncident>> GetMedicalIncidentsByTypeAsync(IncidentType incidentType);
        Task<List<MedicalIncident>> GetMedicalIncidentsByStatusAsync(IncidentStatus status);
        Task<bool> CreateMedicalIncidentAsync(MedicalIncident medicalIncident);
        Task<bool> UpdateMedicalIncidentAsync(MedicalIncident medicalIncident);
        Task<bool> DeleteMedicalIncidentAsync(Guid id);
        Task<List<MedicalIncident>> SearchMedicalIncidentsAsync(string searchTerm);
        Task<Dictionary<IncidentType, int>> GetIncidentStatisticsByTypeAsync();
        Task<Dictionary<string, int>> GetIncidentStatisticsByMonthAsync(int year);
        Task<bool> UpdateIncidentStatusAsync(Guid incidentId, IncidentStatus newStatus, string updatedBy);
        void Dispose();
    }
}
