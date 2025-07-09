using BusinessObjects;
using Repositories;
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
        private readonly IHealthRecordRepository _healthRecordRepository;

        public HealthRecordService()
        {
            _healthRecordRepository = new HealthRecordRepository();
        }

        //1. Get all health records
        public List<HealthRecord> GetHealthRecords()
        {
            return _healthRecordRepository.GetHealthRecords();
        }

        //2. Create a new health record
        public void CreateHealthRecord(HealthRecord healthRecord)
        {
            _healthRecordRepository.CreateHealthRecord(healthRecord);
        }

        //3. Update an existing health record
        public void UpdateHealthRecord(HealthRecord healthRecord)
        {
            _healthRecordRepository.UpdateHealthRecord(healthRecord);
        }

        //4. Delete a health record
        public void DeleteHealthRecord(HealthRecord healthRecord)
        {
            _healthRecordRepository.DeleteHealthRecord(healthRecord);
        }

        //5. Get a health record by ID
        public HealthRecord GetHealthRecordById(int id)
        {
            return _healthRecordRepository.GetHealthRecordById(id);
        }
    }
}