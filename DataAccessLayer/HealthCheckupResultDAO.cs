using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class HealthCheckupResultDAO
    {
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();
        public List<HealthCheckupResult> GetByStudentId(Guid studentId)
        {
            return _context.HealthCheckupResults
                .Include(h => h.ScheduleDetail)
                    .ThenInclude(sd => sd.Student)
                .Where(h => h.ScheduleDetail != null && h.ScheduleDetail.StudentId == studentId)
                .OrderByDescending(h => h.CreateAt)
                .ToList();
        }
        public void Add(HealthCheckupResult result)
        {
            _context.HealthCheckupResults.Add(result);
            _context.SaveChanges();
        }

        public void Update(HealthCheckupResult result)
        {
            _context.HealthCheckupResults.Update(result);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var result = _context.HealthCheckupResults.FirstOrDefault(r => r.Id == id);
            if (result != null)
            {
                _context.HealthCheckupResults.Remove(result);
                _context.SaveChanges();
            }
        }
        public HealthCheckupResult? GetByScheduleDetailId(Guid scheduleDetailId)
        {
            return _context.HealthCheckupResults
                .FirstOrDefault(r => r.ScheduleDetailId == scheduleDetailId);
        }
    }
}
