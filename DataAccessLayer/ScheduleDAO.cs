using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataAccessLayer
{
    public class ScheduleDAO
    {
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();
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
        public Schedule GetScheduleByScheduleId(Guid scheduleId)
        {
            return _context.Schedules
                .FirstOrDefault(s => s.Id == scheduleId);
        }
    }
}
