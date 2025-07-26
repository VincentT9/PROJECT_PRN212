using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class MedicalIncidentRepository : IMedicalIncidentRepository
    {
        private readonly MedicalIncidentDAO _medicalIncidentDAO;

        public MedicalIncidentRepository()
        {
            _medicalIncidentDAO = new MedicalIncidentDAO();
        }

        public async Task<List<MedicalIncident>> GetAllMedicalIncidentsAsync()
        {
            return await _medicalIncidentDAO.GetAllMedicalIncidentsAsync();
        }

        public async Task<MedicalIncident?> GetMedicalIncidentByIdAsync(Guid id)
        {
            return await _medicalIncidentDAO.GetMedicalIncidentByIdAsync(id);
        }

        public async Task<List<MedicalIncident>> GetMedicalIncidentsByStudentIdAsync(Guid studentId)
        {
            return await _medicalIncidentDAO.GetMedicalIncidentsByStudentIdAsync(studentId);
        }

        public async Task<List<MedicalIncident>> GetMedicalIncidentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _medicalIncidentDAO.GetMedicalIncidentsByDateRangeAsync(startDate, endDate);
        }

        public async Task<List<MedicalIncident>> GetMedicalIncidentsByTypeAsync(int incidentType)
        {
            return await _medicalIncidentDAO.GetMedicalIncidentsByTypeAsync(incidentType);
        }

        public async Task<List<MedicalIncident>> GetMedicalIncidentsByStatusAsync(int status)
        {
            return await _medicalIncidentDAO.GetMedicalIncidentsByStatusAsync(status);
        }

        public async Task<bool> CreateMedicalIncidentAsync(MedicalIncident medicalIncident)
        {
            return await _medicalIncidentDAO.CreateMedicalIncidentAsync(medicalIncident);
        }

        public async Task<bool> UpdateMedicalIncidentAsync(MedicalIncident medicalIncident)
        {
            return await _medicalIncidentDAO.UpdateMedicalIncidentAsync(medicalIncident);
        }

        public async Task<bool> DeleteMedicalIncidentAsync(Guid id)
        {
            return await _medicalIncidentDAO.DeleteMedicalIncidentAsync(id);
        }

        public async Task<List<MedicalIncident>> SearchMedicalIncidentsAsync(string searchTerm)
        {
            return await _medicalIncidentDAO.SearchMedicalIncidentsAsync(searchTerm);
        }

        public async Task<Dictionary<int, int>> GetIncidentStatisticsByTypeAsync()
        {
            return await _medicalIncidentDAO.GetIncidentStatisticsByTypeAsync();
        }

        public async Task<Dictionary<string, int>> GetIncidentStatisticsByMonthAsync(int year)
        {
            return await _medicalIncidentDAO.GetIncidentStatisticsByMonthAsync(year);
        }

        public void Dispose()
        {
            _medicalIncidentDAO?.Dispose();
        }
    }
}
