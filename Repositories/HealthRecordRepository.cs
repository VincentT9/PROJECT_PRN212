using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly HealthRecordDAO _healthRecordDAO;

        public HealthRecordRepository()
        {
            _healthRecordDAO = new HealthRecordDAO();
        }

        public async Task<List<HealthRecord>> GetAllHealthRecordsAsync()
        {
            return await _healthRecordDAO.GetAllHealthRecordsAsync();
        }

        public async Task<HealthRecord?> GetHealthRecordByIdAsync(Guid id)
        {
            return await _healthRecordDAO.GetHealthRecordByIdAsync(id);
        }

        public async Task<HealthRecord?> GetHealthRecordByStudentIdAsync(Guid studentId)
        {
            return await _healthRecordDAO.GetHealthRecordByStudentIdAsync(studentId);
        }

        public async Task<List<HealthRecord>> GetHealthRecordsByStudentIdsAsync(List<Guid> studentIds)
        {
            return await _healthRecordDAO.GetHealthRecordsByStudentIdsAsync(studentIds);
        }

        public async Task<bool> CreateHealthRecordAsync(HealthRecord healthRecord)
        {
            return await _healthRecordDAO.CreateHealthRecordAsync(healthRecord);
        }

        public async Task<bool> UpdateHealthRecordAsync(HealthRecord healthRecord)
        {
            return await _healthRecordDAO.UpdateHealthRecordAsync(healthRecord);
        }

        public async Task<bool> DeleteHealthRecordAsync(Guid id)
        {
            return await _healthRecordDAO.DeleteHealthRecordAsync(id);
        }

        public async Task<List<HealthRecord>> SearchHealthRecordsAsync(string searchTerm)
        {
            return await _healthRecordDAO.SearchHealthRecordsAsync(searchTerm);
        }

        public void Dispose()
        {
            _healthRecordDAO?.Dispose();
        }
    }
}
