using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IScheduleRepository
    {
        public List<Schedule> GetSchedules();
        public void CreateSchedule(Schedule schedule);
        public void UpdateSchedule(Schedule schedule);
        public void DeleteSchedule(Schedule schedule);
        public List<ScheduleDetail> GetScheduleDetailsByScheduleId(Guid scheduleId);
        public List<Guid> GetStudentIdsByScheduleId(Guid scheduleId);
    }
}
