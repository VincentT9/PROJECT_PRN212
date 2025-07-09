using BusinessObjects;

namespace Repositories.Interface
{
    public interface IHealthRecordRepository
    {
        public List<HealthRecord> GetHealthRecords();
        public void CreateHealthRecord(HealthRecord healthRecord);
        public void UpdateHealthRecord(HealthRecord healthRecord);
        public void DeleteHealthRecord(HealthRecord healthRecord);
        public HealthRecord GetHealthRecordById(int id);
    }
}