using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IHealthRecordService
    {
        public List<HealthRecord> GetHealthRecords();
        public void CreateHealthRecord(HealthRecord healthRecord);
        public void UpdateHealthRecord(HealthRecord healthRecord);
        public void DeleteHealthRecord(HealthRecord healthRecord);
        public HealthRecord GetHealthRecordById(int id);
    }
}