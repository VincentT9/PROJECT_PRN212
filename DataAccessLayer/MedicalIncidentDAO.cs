using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class MedicalIncidentDAO
    {
        private readonly SwpSchoolMedicalManagementSystemContext _context;

        public MedicalIncidentDAO()
        {
            _context = new SwpSchoolMedicalManagementSystemContext();
        }

        public async Task<List<MedicalIncident>> GetAllMedicalIncidentsAsync()
        {
            return await _context.MedicalIncidents
                .Include(mi => mi.Student)
                .Include(mi => mi.MedicalStaff)
                .Include(mi => mi.MedicalSupplyUsages)
                .ThenInclude(msu => msu.Supply)
                .OrderByDescending(mi => mi.IncidentDate)
                .ToListAsync();
        }

        public async Task<MedicalIncident?> GetMedicalIncidentByIdAsync(Guid id)
        {
            return await _context.MedicalIncidents
                .Include(mi => mi.Student)
                .Include(mi => mi.MedicalStaff)
                .Include(mi => mi.MedicalSupplyUsages)
                .ThenInclude(msu => msu.Supply)
                .FirstOrDefaultAsync(mi => mi.Id == id);
        }

        public async Task<List<MedicalIncident>> GetMedicalIncidentsByStudentIdAsync(Guid studentId)
        {
            return await _context.MedicalIncidents
                .Include(mi => mi.Student)
                .Include(mi => mi.MedicalStaff)
                .Include(mi => mi.MedicalSupplyUsages)
                .ThenInclude(msu => msu.Supply)
                .Where(mi => mi.StudentId == studentId)
                .OrderByDescending(mi => mi.IncidentDate)
                .ToListAsync();
        }

        public async Task<List<MedicalIncident>> GetMedicalIncidentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.MedicalIncidents
                .Include(mi => mi.Student)
                .Include(mi => mi.MedicalStaff)
                .Include(mi => mi.MedicalSupplyUsages)
                .ThenInclude(msu => msu.Supply)
                .Where(mi => mi.IncidentDate >= startDate && mi.IncidentDate <= endDate)
                .OrderByDescending(mi => mi.IncidentDate)
                .ToListAsync();
        }

        public async Task<List<MedicalIncident>> GetMedicalIncidentsByTypeAsync(int incidentType)
        {
            return await _context.MedicalIncidents
                .Include(mi => mi.Student)
                .Include(mi => mi.MedicalStaff)
                .Include(mi => mi.MedicalSupplyUsages)
                .ThenInclude(msu => msu.Supply)
                .Where(mi => mi.IncidentType == incidentType)
                .OrderByDescending(mi => mi.IncidentDate)
                .ToListAsync();
        }

        public async Task<List<MedicalIncident>> GetMedicalIncidentsByStatusAsync(int status)
        {
            return await _context.MedicalIncidents
                .Include(mi => mi.Student)
                .Include(mi => mi.MedicalStaff)
                .Include(mi => mi.MedicalSupplyUsages)
                .ThenInclude(msu => msu.Supply)
                .Where(mi => mi.Status == status)
                .OrderByDescending(mi => mi.IncidentDate)
                .ToListAsync();
        }

        public async Task<bool> CreateMedicalIncidentAsync(MedicalIncident medicalIncident)
        {
            try
            {
                medicalIncident.Id = Guid.NewGuid();

                if (medicalIncident.IncidentDate.Kind != DateTimeKind.Utc)
                    medicalIncident.IncidentDate = DateTime.SpecifyKind(medicalIncident.IncidentDate, DateTimeKind.Utc);
                medicalIncident.CreateAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                medicalIncident.UpdateAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

                _context.MedicalIncidents.Add(medicalIncident);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Debug.WriteLine("Lỗi tại CreateMedicalIncidentAsync " + ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateMedicalIncidentAsync(MedicalIncident medicalIncident)
        {
            try
            {
                var existingIncident = await _context.MedicalIncidents.FindAsync(medicalIncident.Id);
                if (existingIncident == null) return false;

                existingIncident.StudentId = medicalIncident.StudentId;
                existingIncident.MedicalStaffId = medicalIncident.MedicalStaffId;
                existingIncident.IncidentType = medicalIncident.IncidentType;
                existingIncident.IncidentDate = DateTime.SpecifyKind(medicalIncident.IncidentDate, DateTimeKind.Utc);
                existingIncident.Description = medicalIncident.Description;
                existingIncident.ActionsTaken = medicalIncident.ActionsTaken;
                existingIncident.Outcome = medicalIncident.Outcome;
                existingIncident.Status = medicalIncident.Status;
                existingIncident.UpdatedBy = medicalIncident.UpdatedBy;
                existingIncident.UpdateAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi cập nhật sự kiện: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        public async Task<bool> DeleteMedicalIncidentAsync(Guid id)
        {
            try
            {
                var medicalIncident = await _context.MedicalIncidents.FindAsync(id);
                if (medicalIncident == null) return false;

                _context.MedicalIncidents.Remove(medicalIncident);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                Debug.WriteLine($"Lỗi xóa sự kiện y tế với ID {id}");
                return false;
            }
        }

        public async Task<List<MedicalIncident>> SearchMedicalIncidentsAsync(string searchTerm)
        {
            var incidents = await _context.MedicalIncidents
                .Include(mi => mi.Student)
                .Include(mi => mi.MedicalStaff)
                .Include(mi => mi.MedicalSupplyUsages)
                .ThenInclude(msu => msu.Supply)
                .Where(mi => mi.Student!.FullName!.Contains(searchTerm) ||
                           mi.MedicalStaff!.FullName!.Contains(searchTerm) ||
                           (!string.IsNullOrEmpty(mi.Description) && mi.Description.Contains(searchTerm)) ||
                           (!string.IsNullOrEmpty(mi.ActionsTaken) && mi.ActionsTaken.Contains(searchTerm)) ||
                           (!string.IsNullOrEmpty(mi.Outcome) && mi.Outcome.Contains(searchTerm)))
                .OrderByDescending(mi => mi.IncidentDate)
                .ToListAsync();

            return incidents;
        }

        // Thống kê số lượng sự cố theo loại
        public async Task<Dictionary<int, int>> GetIncidentStatisticsByTypeAsync()
        {
            var statistics = await _context.MedicalIncidents
                .GroupBy(mi => mi.IncidentType)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Type, x => x.Count);

            return statistics;
        }

        // Thống kê số lượng sự cố theo tháng
        public async Task<Dictionary<string, int>> GetIncidentStatisticsByMonthAsync(int year)
        {
            var statistics = await _context.MedicalIncidents
                .Where(mi => mi.IncidentDate.Year == year)
                .GroupBy(mi => mi.IncidentDate.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => $"Tháng {x.Month}", x => x.Count);

            return statistics;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
