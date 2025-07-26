using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IHealthRecordService
    {
        public List<HealthRecord> GetAllHealthRecordAsync();
        public HealthRecord GetHealthRecordByIdAsync(Guid healthRecordId);
        public HealthRecord? GetHealthRecordByStudentIdAsync(Guid studentId);
        public bool CreateHealthRecordAsync(HealthRecord healthRecord);
        public bool UpdateHealthRecordAsync(HealthRecord healthRecord);
        public bool DeleteHealthRecordAsync(Guid HealthRecordId);
    }
}
