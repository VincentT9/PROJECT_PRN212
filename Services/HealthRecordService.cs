using BusinessObjects;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _healthRecordRepository;

        public HealthRecordService(IHealthRecordRepository healthRecordRepository)
        {
            _healthRecordRepository = healthRecordRepository;
        }

        public async Task<List<HealthRecord>> GetAllHealthRecordsAsync()
        {
            return await _healthRecordRepository.GetAllHealthRecordsAsync();
        }

        public async Task<HealthRecord?> GetHealthRecordByIdAsync(Guid id)
        {
            return await _healthRecordRepository.GetHealthRecordByIdAsync(id);
        }

        public async Task<HealthRecord?> GetHealthRecordByStudentIdAsync(Guid studentId)
        {
            return await _healthRecordRepository.GetHealthRecordByStudentIdAsync(studentId);
        }

        public async Task<List<HealthRecord>> GetHealthRecordsByStudentIdsAsync(List<Guid> studentIds)
        {
            return await _healthRecordRepository.GetHealthRecordsByStudentIdsAsync(studentIds);
        }

        public async Task<bool> CreateHealthRecordAsync(HealthRecord healthRecord)
        {
            // Validation logic
            if (string.IsNullOrWhiteSpace(healthRecord.Height) &&
                string.IsNullOrWhiteSpace(healthRecord.Weight) &&
                string.IsNullOrWhiteSpace(healthRecord.BloodType))
            {
                return false; // At least one field must be filled
            }

            return await _healthRecordRepository.CreateHealthRecordAsync(healthRecord);
        }

        public async Task<bool> UpdateHealthRecordAsync(HealthRecord healthRecord)
        {
            var existingRecord = await _healthRecordRepository.GetHealthRecordByIdAsync(healthRecord.Id);
            if (existingRecord == null)
            {
                return false;
            }

            return await _healthRecordRepository.UpdateHealthRecordAsync(healthRecord);
        }

        public async Task<bool> DeleteHealthRecordAsync(Guid id)
        {
            return await _healthRecordRepository.DeleteHealthRecordAsync(id);
        }

        public async Task<List<HealthRecord>> SearchHealthRecordsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllHealthRecordsAsync();
            }

            return await _healthRecordRepository.SearchHealthRecordsAsync(searchTerm);
        }

        public async Task<bool> CreateOrUpdateHealthRecordAsync(HealthRecord healthRecord)
        {
            var existingRecord = await _healthRecordRepository.GetHealthRecordByStudentIdAsync(healthRecord.StudentId.Value);

            if (existingRecord == null)
            {
                return await CreateHealthRecordAsync(healthRecord);
            }
            else
            {
                healthRecord.Id = existingRecord.Id;
                return await UpdateHealthRecordAsync(healthRecord);
            }
        }

        public void Dispose()
        {
            _healthRecordRepository?.Dispose();
        }
    }
}
