using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class HealthRecordDAO
    {
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();
        public List<HealthRecord> GetAllHealthRecordAsync()
        {
            return _context.HealthRecords.ToList();
        }

        public HealthRecord? GetHealthRecordByIdAsync(Guid healthRecordId)
        {
            return _context.HealthRecords.FirstOrDefault(hr => hr.Id == healthRecordId);
        }
        public HealthRecord GetHealthRecordByStudentIdAsync(Guid studentId)
        {
            return _context.HealthRecords.FirstOrDefault(hr => hr.StudentId == studentId);
        }
        public bool CreateHealthRecordAsync(HealthRecord healthRecord)
        {
            if (healthRecord == null) return false;
            HealthRecord existingHealthRecord = _context.HealthRecords.FirstOrDefault(hr => hr.Id == healthRecord.Id);
            if (existingHealthRecord != null) return false;

            _context.HealthRecords.Add(healthRecord);
            int ret = _context.SaveChanges();

            return ret > 0;
        }
        public bool UpdateHealthRecordAsync(HealthRecord healthRecord)
        {
            if (healthRecord == null) return false;
            HealthRecord existingHealthRecord = _context.HealthRecords.FirstOrDefault(hr => hr.Id == healthRecord.Id);
            if (existingHealthRecord == null) return false;
            // Update properties manually to avoid tracking error
            existingHealthRecord.Height = healthRecord.Height;
            existingHealthRecord.Weight = healthRecord.Weight;
            existingHealthRecord.BloodType = healthRecord.BloodType;
            existingHealthRecord.Allergies = healthRecord.Allergies;
            existingHealthRecord.ChronicDiseases = healthRecord.ChronicDiseases;
            existingHealthRecord.PastMedicalHistory = healthRecord.PastMedicalHistory;
            existingHealthRecord.VisionLeft = healthRecord.VisionLeft;
            existingHealthRecord.VisionRight = healthRecord.VisionRight;
            existingHealthRecord.HearingLeft = healthRecord.HearingLeft;
            existingHealthRecord.HearingRight = healthRecord.HearingRight;
            existingHealthRecord.VaccinationHistory = healthRecord.VaccinationHistory;
            existingHealthRecord.OtherNotes = healthRecord.OtherNotes;
            existingHealthRecord.UpdatedBy = healthRecord.UpdatedBy;
            existingHealthRecord.UpdateAt = DateTime.UtcNow;
            int ret = _context.SaveChanges();
            return ret > 0;
        }
        public bool DeleteHealthRecordAsync(Guid HealthRecordId)
        {
            HealthRecord existingHealthRecord = _context.HealthRecords.FirstOrDefault(hr => hr.Id == HealthRecordId);
            if (existingHealthRecord == null) return false;
            _context.HealthRecords.Remove(existingHealthRecord);
            int ret = _context.SaveChanges();
            return ret > 0;

        }
    }
}
