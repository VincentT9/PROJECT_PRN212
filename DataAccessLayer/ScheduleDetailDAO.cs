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
        public bool ExistsInSchedule(Guid studentId, Guid scheduleId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.ScheduleDetails.Any(sd => sd.StudentId == studentId && sd.ScheduleId == scheduleId);
        }

        public void AddScheduleDetail(ScheduleDetail detail)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            context.ScheduleDetails.Add(detail);
            context.SaveChanges();
        }

        public List<Student> GetStudentsByScheduleId(Guid scheduleId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.ScheduleDetails
                           .Where(sd => sd.ScheduleId == scheduleId && sd.StudentId != null)
                           .Include(sd => sd.Student) // load Student
                           .Select(sd => sd.Student!)
                           .ToList();
        }
        public bool IsStudentInSchedule(Guid studentId, Guid scheduleId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.ScheduleDetails.Any(sd =>
                sd.StudentId == studentId && sd.ScheduleId == scheduleId);
        }
        public List<ScheduleDetail> GetByScheduleId(Guid scheduleId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.ScheduleDetails
                .Include(sd => sd.Student)
                .Where(sd => sd.ScheduleId == scheduleId)
                .ToList();
        }
        public ScheduleDetail? GetByStudentAndSchedule(Guid studentId, Guid scheduleId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.ScheduleDetails
                .FirstOrDefault(sd => sd.StudentId == studentId && sd.ScheduleId == scheduleId);
        }
    }
}
