using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class ScheduleDAO
    {
        public List<Schedule> GetSchedules()
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.Schedules.ToList();
        }

        public List<Schedule> GetAllSchedules()
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.Schedules.ToList();
        }

        public void CreateSchedule(Schedule schedule)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            var existingSchedule = context.Schedules.FirstOrDefault(s => s.Id == schedule.Id);
            if (existingSchedule != null)
            {
                return;
            }
            context.Schedules.Add(schedule);
            context.SaveChanges();
        }

        public void UpdateSchedule(Schedule schedule)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            var existingSchedule = context.Schedules.FirstOrDefault(s => s.Id == schedule.Id);
            if (existingSchedule != null)
            {
                context.Entry(existingSchedule).CurrentValues.SetValues(schedule);
                context.SaveChanges();
            }
        }

        public void DeleteSchedule(Schedule schedule)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            var existingSchedule = context.Schedules.FirstOrDefault(s => s.Id == schedule.Id);
            if (existingSchedule != null)
            {
                context.Schedules.Remove(existingSchedule);
                context.SaveChanges();
            }
        }

        public Schedule GetScheduleByScheduleId(Guid scheduleId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.Schedules
                .FirstOrDefault(s => s.Id == scheduleId);
        }

        public List<Schedule> GetActiveSchedules()
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
         return context.Schedules
                .Include(s => s.Campaign)
                .Where(s => s.ScheduledDate >= DateTimeOffset.UtcNow )
                .OrderBy(s => s.ScheduledDate)
                .ToList();
        }

        public List<ScheduleDetail> GetScheduleDetailsByScheduleId(Guid scheduleId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.ScheduleDetails
                .Where(sd => sd.ScheduleId == scheduleId)
                .ToList();
        }

        public List<Guid> GetStudentIdsByScheduleId(Guid scheduleId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.ScheduleDetails
                .Where(sd => sd.ScheduleId == scheduleId)
                .Select(sd => sd.StudentId)
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList();
        }

        public void UpdateStudentVaccinationStatus(Guid studentId, Guid scheduleId, string status)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            var scheduleDetail = context.ScheduleDetails
                .FirstOrDefault(sd => sd.StudentId == studentId && sd.ScheduleId == scheduleId);
            if (scheduleDetail != null)
            {
                scheduleDetail.Status = status;
                context.SaveChanges();
            }
        }
    }
}
