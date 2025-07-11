using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class ScheduleDetailDAO
    {
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();
        public bool ExistsInSchedule(Guid studentId, Guid scheduleId)
        {
            return _context.ScheduleDetails.Any(sd => sd.StudentId == studentId && sd.ScheduleId == scheduleId);
        }

        public void AddScheduleDetail(ScheduleDetail detail)
        {
            _context.ScheduleDetails.Add(detail);
            _context.SaveChanges();
        }

        public List<Student> GetStudentsByScheduleId(Guid scheduleId)
        {
            return _context.ScheduleDetails
                           .Where(sd => sd.ScheduleId == scheduleId && sd.StudentId != null)
                           .Include(sd => sd.Student) // load Student
                           .Select(sd => sd.Student!)
                           .ToList();
        }
        public bool IsStudentInSchedule(Guid studentId, Guid scheduleId)
        {
            return _context.ScheduleDetails.Any(sd =>
                sd.StudentId == studentId && sd.ScheduleId == scheduleId);
        }
    }
}
