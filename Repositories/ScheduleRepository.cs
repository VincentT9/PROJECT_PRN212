using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        ScheduleDAO scheduleDAO = new ScheduleDAO();
        public void CreateSchedule(Schedule schedule) => scheduleDAO.CreateSchedule(schedule);


        public void DeleteSchedule(Schedule schedule) => scheduleDAO.DeleteSchedule(schedule);

        public List<ScheduleDetail> GetScheduleDetailsByScheduleId(Guid scheduleId) => scheduleDAO.GetScheduleDetailsByScheduleId(scheduleId);


        public List<Schedule> GetSchedules() => scheduleDAO.GetSchedules();


        public List<Guid> GetStudentIdsByScheduleId(Guid scheduleId) => scheduleDAO.GetStudentIdsByScheduleId(scheduleId);


        public void UpdateSchedule(Schedule schedule) => scheduleDAO.UpdateSchedule(schedule);

    }
}
