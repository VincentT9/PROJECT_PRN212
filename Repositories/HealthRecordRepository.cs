using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;

namespace Repositories
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        HealthRecordDAO _healthRecordDAO = new HealthRecordDAO();

        //1. Get all health records
        public List<HealthRecord> GetHealthRecords()
        {
            return _healthRecordDAO.GetHealthRecords();
        }

        //2. Create a new health record
        public void CreateHealthRecord(HealthRecord healthRecord)
        {
            _healthRecordDAO.CreateHealthRecord(healthRecord);
        }

        //3. Update an existing health record
        public void UpdateHealthRecord(HealthRecord healthRecord)
        {
            _healthRecordDAO.UpdateHealthRecord(healthRecord);
        }

        //4. Delete a health record
        public void DeleteHealthRecord(HealthRecord healthRecord)
        {
            _healthRecordDAO.DeleteHealthRecord(healthRecord);
        }

        //5. Get a health record by ID
        public HealthRecord GetHealthRecordById(int id)
        {
            return _healthRecordDAO.GetHealthRecordById(id);
        }
    }
}