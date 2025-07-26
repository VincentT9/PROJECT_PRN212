using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IHealthRecordService
    {
        Task<List<HealthRecord>> GetAllHealthRecordsAsync();
        Task<HealthRecord?> GetHealthRecordByIdAsync(Guid id);
        Task<HealthRecord?> GetHealthRecordByStudentIdAsync(Guid studentId);
        Task<List<HealthRecord>> GetHealthRecordsByStudentIdsAsync(List<Guid> studentIds);
        Task<bool> CreateHealthRecordAsync(HealthRecord healthRecord);
        Task<bool> UpdateHealthRecordAsync(HealthRecord healthRecord);
        Task<bool> DeleteHealthRecordAsync(Guid id);
        Task<List<HealthRecord>> SearchHealthRecordsAsync(string searchTerm);
        Task<bool> CreateOrUpdateHealthRecordAsync(HealthRecord healthRecord);
        void Dispose();
    }
}
