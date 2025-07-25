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
        public List<HealthCheckupResult> GetByStudentId(Guid studentId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.HealthCheckupResults
                .Include(h => h.ScheduleDetail)
                    .ThenInclude(sd => sd.Student)
                .Where(h => h.ScheduleDetail != null && h.ScheduleDetail.StudentId == studentId)
                .OrderByDescending(h => h.CreateAt)
                .ToList();
        }
        public void Add(HealthCheckupResult result)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            context.HealthCheckupResults.Add(result);
            context.SaveChanges();
        }

        public void Update(HealthCheckupResult result)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            context.HealthCheckupResults.Update(result);
            context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            var result = context.HealthCheckupResults.FirstOrDefault(r => r.Id == id);
            if (result != null)
            {
                context.HealthCheckupResults.Remove(result);
                context.SaveChanges();
            }
        }
        public HealthCheckupResult? GetByScheduleDetailId(Guid scheduleDetailId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.HealthCheckupResults
                .FirstOrDefault(r => r.ScheduleDetailId == scheduleDetailId);
        }
    }
}
