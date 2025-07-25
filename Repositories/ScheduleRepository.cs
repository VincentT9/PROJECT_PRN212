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
        public void CreateSchedule(Schedule schedule)
        {
            _scheduleDAO.CreateSchedule(schedule);
        }

        public List<Schedule> GetAllSchedules()
        {
            return _scheduleDAO.GetAllSchedules();
        }
        public Schedule GetScheduleByScheduleId(Guid scheduleId)
        {
            return _scheduleDAO.GetScheduleByScheduleId(scheduleId);
        }
        public List<Schedule> GetActiveSchedules()
        {
            return _scheduleDAO.GetActiveSchedules();
        }
    }
}
