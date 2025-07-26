using BusinessObjects;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class HealthRecordService : IHealthRecordService
    {
        IHealthRecordRepository _healthRecordRepository;
        public HealthRecordService()
        {
            _healthRecordRepository = new HealthRecordRepository();
        }

        public bool CreateHealthRecordAsync(HealthRecord healthRecord)
        {
            return _healthRecordRepository.CreateHealthRecordAsync(healthRecord);
        }

        public bool DeleteHealthRecordAsync(Guid HealthRecordId)
        {
            return _healthRecordRepository.DeleteHealthRecordAsync(HealthRecordId);
        }

        public List<HealthRecord> GetAllHealthRecordAsync()
        {
            return _healthRecordRepository.GetAllHealthRecordAsync();
        }

        public HealthRecord GetHealthRecordByIdAsync(Guid healthRecordId)
        {
            return _healthRecordRepository.GetHealthRecordByIdAsync(healthRecordId);
        }

        public HealthRecord? GetHealthRecordByStudentIdAsync(Guid studentId)
        {
            return _healthRecordRepository.GetHealthRecordByStudentIdAsync(studentId);
            }

        public bool UpdateHealthRecordAsync(HealthRecord healthRecord)
        {
            return _healthRecordRepository.UpdateHealthRecordAsync(healthRecord);
        }
    }
}
