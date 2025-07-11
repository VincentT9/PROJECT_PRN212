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
    }
}
