using BusinessObjects;
using Repositories.Interface;
using Services.Interface;
using SchoolMedicalManagementSystem.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories;

namespace Services
{
    public class MedicalIncidentService : IMedicalIncidentService
    {
        private readonly IMedicalIncidentRepository _medicalIncidentRepository;

        public MedicalIncidentService()
        {
            _medicalIncidentRepository = new MedicalIncidentRepository();
        }
        public MedicalIncidentService(IMedicalIncidentRepository medicalIncidentRepository)
        {
            _medicalIncidentRepository = medicalIncidentRepository;
        }

        public async Task<List<MedicalIncident>> GetAllMedicalIncidentsAsync()
        {
            return await _medicalIncidentRepository.GetAllMedicalIncidentsAsync();
        }

        public async Task<MedicalIncident?> GetMedicalIncidentByIdAsync(Guid id)
        {
            return await _medicalIncidentRepository.GetMedicalIncidentByIdAsync(id);
        }

        public async Task<List<MedicalIncident>> GetMedicalIncidentsByStudentIdAsync(Guid studentId)
        {
            return await _medicalIncidentRepository.GetMedicalIncidentsByStudentIdAsync(studentId);
        }

        public async Task<List<MedicalIncident>> GetMedicalIncidentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _medicalIncidentRepository.GetMedicalIncidentsByDateRangeAsync(startDate, endDate);
        }

        public async Task<List<MedicalIncident>> GetMedicalIncidentsByTypeAsync(IncidentType incidentType)
        {
            return await _medicalIncidentRepository.GetMedicalIncidentsByTypeAsync((int)incidentType);
        }

        public async Task<List<MedicalIncident>> GetMedicalIncidentsByStatusAsync(IncidentStatus status)
        {
            return await _medicalIncidentRepository.GetMedicalIncidentsByStatusAsync((int)status);
        }

        public async Task<bool> CreateMedicalIncidentAsync(MedicalIncident medicalIncident)
        {
            if (medicalIncident.Description == null || !medicalIncident.Description.Any())
            {
                return false; 
            }

            if (medicalIncident.StudentId == null || medicalIncident.StudentId == Guid.Empty)
            {
                return false; 
            }

           
            if (medicalIncident.Status == 0)
            {
                medicalIncident.Status = (int)IncidentStatus.Reported;
            }

            return await _medicalIncidentRepository.CreateMedicalIncidentAsync(medicalIncident);
        }

        public async Task<bool> UpdateMedicalIncidentAsync(MedicalIncident medicalIncident)
        {
            var existingIncident = await _medicalIncidentRepository.GetMedicalIncidentByIdAsync(medicalIncident.Id);
            if (existingIncident == null)
            {
                return false;
            }

            return await _medicalIncidentRepository.UpdateMedicalIncidentAsync(medicalIncident);
        }

        public async Task<bool> DeleteMedicalIncidentAsync(Guid id)
        {
            return await _medicalIncidentRepository.DeleteMedicalIncidentAsync(id);
        }

        public async Task<List<MedicalIncident>> SearchMedicalIncidentsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllMedicalIncidentsAsync();
            }

            return await _medicalIncidentRepository.SearchMedicalIncidentsAsync(searchTerm);
        }

        public async Task<Dictionary<IncidentType, int>> GetIncidentStatisticsByTypeAsync()
        {
            var stats = await _medicalIncidentRepository.GetIncidentStatisticsByTypeAsync();
            var result = new Dictionary<IncidentType, int>();

            foreach (var stat in stats)
            {
                result[(IncidentType)stat.Key] = stat.Value;
            }

            return result;
        }

        public async Task<Dictionary<string, int>> GetIncidentStatisticsByMonthAsync(int year)
        {
            return await _medicalIncidentRepository.GetIncidentStatisticsByMonthAsync(year);
        }

        public async Task<bool> UpdateIncidentStatusAsync(Guid incidentId, IncidentStatus newStatus, string updatedBy)
        {
            var incident = await _medicalIncidentRepository.GetMedicalIncidentByIdAsync(incidentId);
            if (incident == null)
            {
                return false;
            }

            incident.Status = (int)newStatus;
            incident.UpdatedBy = updatedBy;

            return await _medicalIncidentRepository.UpdateMedicalIncidentAsync(incident);
        }

        public void Dispose()
        {
            _medicalIncidentRepository?.Dispose();
        }
    }
}
