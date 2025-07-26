using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IMedicalIncidentRepository
    {
        Task<List<MedicalIncident>> GetAllMedicalIncidentsAsync();
        Task<MedicalIncident?> GetMedicalIncidentByIdAsync(Guid id);
        Task<List<MedicalIncident>> GetMedicalIncidentsByStudentIdAsync(Guid studentId);
        Task<List<MedicalIncident>> GetMedicalIncidentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<MedicalIncident>> GetMedicalIncidentsByTypeAsync(int incidentType);
        Task<List<MedicalIncident>> GetMedicalIncidentsByStatusAsync(int status);
        Task<bool> CreateMedicalIncidentAsync(MedicalIncident medicalIncident);
        Task<bool> UpdateMedicalIncidentAsync(MedicalIncident medicalIncident);
        Task<bool> DeleteMedicalIncidentAsync(Guid id);
        Task<List<MedicalIncident>> SearchMedicalIncidentsAsync(string searchTerm);
        Task<Dictionary<int, int>> GetIncidentStatisticsByTypeAsync();
        Task<Dictionary<string, int>> GetIncidentStatisticsByMonthAsync(int year);
        void Dispose();
    }
}
