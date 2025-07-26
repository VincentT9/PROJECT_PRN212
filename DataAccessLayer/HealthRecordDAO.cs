using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class HealthRecordDAO
    {
        private readonly SwpSchoolMedicalManagementSystemContext _context;

        public HealthRecordDAO()
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();
        public List<HealthRecord> GetAllHealthRecordAsync()
        {
            _context = new SwpSchoolMedicalManagementSystemContext();
            return _context.HealthRecords.ToList();
        }

        public async Task<List<HealthRecord>> GetAllHealthRecordsAsync()
        public HealthRecord? GetHealthRecordByIdAsync(Guid healthRecordId)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Student)
                .OrderByDescending(hr => hr.CreateAt)
                .ToListAsync();
            return _context.HealthRecords.FirstOrDefault(hr => hr.Id == healthRecordId);
        }

        public async Task<HealthRecord?> GetHealthRecordByIdAsync(Guid id)
        public HealthRecord GetHealthRecordByStudentIdAsync(Guid studentId)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Student)
                .FirstOrDefaultAsync(hr => hr.Id == id);
            return _context.HealthRecords.FirstOrDefault(hr => hr.StudentId == studentId);
        }

        public async Task<HealthRecord?> GetHealthRecordByStudentIdAsync(Guid studentId)
        public bool CreateHealthRecordAsync(HealthRecord healthRecord)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Student)
                .FirstOrDefaultAsync(hr => hr.StudentId == studentId);
        }
            if (healthRecord == null) return false;
            HealthRecord existingHealthRecord = _context.HealthRecords.FirstOrDefault(hr => hr.Id == healthRecord.Id);
            if(existingHealthRecord != null) return false;

        public async Task<List<HealthRecord>> GetHealthRecordsByStudentIdsAsync(List<Guid> studentIds)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Student)
                .Where(hr => studentIds.Contains(hr.StudentId.Value))
                .ToListAsync();
        }

        public async Task<bool> CreateHealthRecordAsync(HealthRecord healthRecord)
        {
            try
            {
                healthRecord.Id = Guid.NewGuid();
                healthRecord.CreateAt = DateTime.Now;
                healthRecord.UpdateAt = DateTime.Now;

                _context.HealthRecords.Add(healthRecord);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
            int ret = _context.SaveChanges();

        public async Task<bool> UpdateHealthRecordAsync(HealthRecord healthRecord)
        {
            try
            {
                var existingRecord = await _context.HealthRecords.FindAsync(healthRecord.Id);
                if (existingRecord == null) return false;

                existingRecord.Height = healthRecord.Height;
                existingRecord.Weight = healthRecord.Weight;
                existingRecord.BloodType = healthRecord.BloodType;
                existingRecord.Allergies = healthRecord.Allergies;
                existingRecord.ChronicDiseases = healthRecord.ChronicDiseases;
                existingRecord.PastMedicalHistory = healthRecord.PastMedicalHistory;
                existingRecord.VisionLeft = healthRecord.VisionLeft;
                existingRecord.VisionRight = healthRecord.VisionRight;
                existingRecord.HearingLeft = healthRecord.HearingLeft;
                existingRecord.HearingRight = healthRecord.HearingRight;
                existingRecord.VaccinationHistory = healthRecord.VaccinationHistory;
                existingRecord.OtherNotes = healthRecord.OtherNotes;
                existingRecord.UpdatedBy = healthRecord.UpdatedBy;
                existingRecord.UpdateAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
            return ret > 0;
        }

        public async Task<bool> DeleteHealthRecordAsync(Guid id)
        {
            try
        public bool UpdateHealthRecordAsync(HealthRecord healthRecord)
            {
                var healthRecord = await _context.HealthRecords.FindAsync(id);
                if (healthRecord == null) return false;

                _context.HealthRecords.Remove(healthRecord);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
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

        public async Task<List<HealthRecord>> SearchHealthRecordsAsync(string searchTerm)
        public bool DeleteHealthRecordAsync(Guid HealthRecordId)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Student)
                .Where(hr => hr.Student!.FullName!.Contains(searchTerm) ||
                           hr.BloodType!.Contains(searchTerm) ||
                           hr.Allergies!.Contains(searchTerm) ||
                           hr.ChronicDiseases!.Contains(searchTerm))
                .ToListAsync();
        }
            HealthRecord existingHealthRecord = _context.HealthRecords.FirstOrDefault(hr => hr.Id == HealthRecordId);
            if (existingHealthRecord == null) return false;
            _context.HealthRecords.Remove(existingHealthRecord);
            int ret = _context.SaveChanges();
            return ret > 0;

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
