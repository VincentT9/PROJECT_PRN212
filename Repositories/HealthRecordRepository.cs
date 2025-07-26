using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        HealthRecordDAO healthRecordDAO = new HealthRecordDAO();
        public bool CreateHealthRecordAsync(HealthRecord healthRecord)
        {
            return healthRecordDAO.CreateHealthRecordAsync(healthRecord);
        }

        public bool DeleteHealthRecordAsync(Guid HealthRecordId)
        {
            return healthRecordDAO.DeleteHealthRecordAsync(HealthRecordId);
        }

        public List<HealthRecord> GetAllHealthRecordAsync()
        {
            return healthRecordDAO.GetAllHealthRecordAsync();
        }

        public HealthRecord GetHealthRecordByIdAsync(Guid healthRecordId)
        {
            return healthRecordDAO.GetHealthRecordByIdAsync(healthRecordId);
        }

        public HealthRecord? GetHealthRecordByStudentIdAsync(Guid studentId)
        {
            return healthRecordDAO.GetHealthRecordByStudentIdAsync(studentId);
        }

        public bool UpdateHealthRecordAsync(HealthRecord healthRecord)
        {
            return healthRecordDAO.UpdateHealthRecordAsync(healthRecord);
        }
    }
}
