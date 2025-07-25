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
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();
        
        public List<Schedule> GetSchedules()
        {
            return _context.Schedules.ToList();
        }
        
        public List<Schedule> GetAllSchedules()
        {
            return _context.Schedules.ToList();
        }
        
        public void CreateSchedule(Schedule schedule)
        {
            var existingSchedule = _context.Schedules.FirstOrDefault(s => s.Id == schedule.Id);
            if (existingSchedule != null)
            {
                return;
            }
            _context.Schedules.Add(schedule);
            _context.SaveChanges();
        }
        
        public void UpdateSchedule(Schedule schedule)
        {
            var existingSchedule = _context.Schedules.FirstOrDefault(s => s.Id == schedule.Id);
            if (existingSchedule != null)
            {
                _context.Entry(existingSchedule).CurrentValues.SetValues(schedule);
                _context.SaveChanges();
            }
        }
        
        public void DeleteSchedule(Schedule schedule)
        {
            var existingSchedule = _context.Schedules.FirstOrDefault(s => s.Id == schedule.Id);
            if (existingSchedule != null)
            {
                _context.Schedules.Remove(existingSchedule);
                _context.SaveChanges();
            }
        }
        
        public Schedule GetScheduleByScheduleId(Guid scheduleId)
        {
            return _context.Schedules
                .FirstOrDefault(s => s.Id == scheduleId);
        }
        
        public List<Schedule> GetActiveSchedules()
        {
            return _context.Schedules
                .Include(s => s.Campaign)
                .Where(s => s.ScheduledDate >= DateTime.Now)
                .OrderBy(s => s.ScheduledDate)
                .ToList();
        }
        
        public List<ScheduleDetail> GetScheduleDetailsByScheduleId(Guid scheduleId)
        {
            return _context.ScheduleDetails
                .Where(sd => sd.ScheduleId == scheduleId)
                .ToList();
        }
        
        public List<Guid> GetStudentIdsByScheduleId(Guid scheduleId)
        {
            return _context.ScheduleDetails
                .Where(sd => sd.ScheduleId == scheduleId)
                .Select(sd => sd.StudentId)
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList();
        }
        
        public void UpdateStudentVaccinationStatus(Guid studentId, Guid scheduleId, string status)
        {
            var scheduleDetail = _context.ScheduleDetails
                .FirstOrDefault(sd => sd.StudentId == studentId && sd.ScheduleId == scheduleId);
            if (scheduleDetail != null)
            {
                scheduleDetail.Status = status;
                _context.SaveChanges();
            }
        }
    }
}
