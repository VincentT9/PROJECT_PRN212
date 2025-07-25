using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;

namespace Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        ScheduleDAO _scheduleDAO = new ScheduleDAO();
        
        public List<Schedule> GetSchedules()
        {
            return _scheduleDAO.GetSchedules();
        }
        
        public List<Schedule> GetAllSchedules()
        {
            return _scheduleDAO.GetAllSchedules();
        }
        
        public void CreateSchedule(Schedule schedule)
        {
            _scheduleDAO.CreateSchedule(schedule);
        }
        
        public void UpdateSchedule(Schedule schedule)
        {
            _scheduleDAO.UpdateSchedule(schedule);
        }
        
        public void DeleteSchedule(Schedule schedule)
        {
            _scheduleDAO.DeleteSchedule(schedule);
        }
        
        public Schedule GetScheduleByScheduleId(Guid scheduleId)
        {
            return _scheduleDAO.GetScheduleByScheduleId(scheduleId);
        }
        
        public List<Schedule> GetActiveSchedules()
        {
            return _scheduleDAO.GetActiveSchedules();
        }
        
        public List<ScheduleDetail> GetScheduleDetailsByScheduleId(Guid scheduleId)
        {
            return _scheduleDAO.GetScheduleDetailsByScheduleId(scheduleId);
        }
        
        public List<Guid> GetStudentIdsByScheduleId(Guid scheduleId)
        {
            return _scheduleDAO.GetStudentIdsByScheduleId(scheduleId);
        }
    }
}
