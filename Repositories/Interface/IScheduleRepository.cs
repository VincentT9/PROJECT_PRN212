using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories.Interface
{
    public interface IScheduleRepository
    {
        public List<Schedule> GetSchedules();
        public List<Schedule> GetAllSchedules();
        public void CreateSchedule(Schedule schedule);
        public void UpdateSchedule(Schedule schedule);
        public void DeleteSchedule(Schedule schedule);
        public Schedule GetScheduleByScheduleId(Guid scheduleId);
        public List<Schedule> GetActiveSchedules();
        public List<ScheduleDetail> GetScheduleDetailsByScheduleId(Guid scheduleId);
        public List<Guid> GetStudentIdsByScheduleId(Guid scheduleId);
    }
}
