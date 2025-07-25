using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class HealthRecordDAO
    {
        private readonly SwpSchoolMedicalManagementSystemContext _context;

        public HealthRecordDAO()
        {
            _context = new SwpSchoolMedicalManagementSystemContext();
        }

        public async Task<List<HealthRecord>> GetAllHealthRecordsAsync()
        {
            return await _context.HealthRecords
                .Include(hr => hr.Student)
                .OrderByDescending(hr => hr.CreateAt)
                .ToListAsync();
        }

        public async Task<HealthRecord?> GetHealthRecordByIdAsync(Guid id)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Student)
                .FirstOrDefaultAsync(hr => hr.Id == id);
        }

        public async Task<HealthRecord?> GetHealthRecordByStudentIdAsync(Guid studentId)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Student)
                .FirstOrDefaultAsync(hr => hr.StudentId == studentId);
        }

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
        }

        public async Task<bool> DeleteHealthRecordAsync(Guid id)
        {
            try
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
        }

        public async Task<List<HealthRecord>> SearchHealthRecordsAsync(string searchTerm)
        {
            return await _context.HealthRecords
                .Include(hr => hr.Student)
                .Where(hr => hr.Student!.FullName!.Contains(searchTerm) ||
                           hr.BloodType!.Contains(searchTerm) ||
                           hr.Allergies!.Contains(searchTerm) ||
                           hr.ChronicDiseases!.Contains(searchTerm))
                .ToListAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
