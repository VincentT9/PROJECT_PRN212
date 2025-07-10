using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ScheduleDAO
    {
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();
        //1. Get all schedules
        public List<Schedule> GetSchedules()
        {
            return _context.Schedules.ToList();
        }
        //2. Create a new schedule
        public void CreateSchedule(Schedule schedule)
        {
            try
            {
                _context.Schedules.Add(schedule);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //3. Update an existing schedule
        public void UpdateSchedule(Schedule schedule)
        {
            try
            {
                _context.Entry<Schedule>(schedule).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //4. Delete a schedule
        public void DeleteSchedule(Schedule schedule)
        {
            try
            {
                var existSchedule = _context.Schedules.SingleOrDefault(s => s.Id == schedule.Id);
                if (existSchedule != null)
                {
                    _context.Schedules.Remove(existSchedule);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //Get scheduleDetails by scheduleId
        public List<ScheduleDetail> GetScheduleDetailsByScheduleId(Guid scheduleId)
        {
            return _context.ScheduleDetails.Where(sd => sd.ScheduleId == scheduleId).ToList();
        }
        //Get studetnId by scheduleId
        public List<Guid> GetStudentIdsByScheduleId(Guid scheduleId)
        {
            return _context.ScheduleDetails
                .Where(sd => sd.ScheduleId == scheduleId && sd.StudentId.HasValue)
                .Select(sd => sd.StudentId.Value)
                .ToList();
        }
    }
}
