using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services.Interface
{
    public interface IScheduleService
    {
        public void CreateSchedule(Schedule schedule);
        public List<Schedule> GetAllSchedules();
        public Schedule GetScheduleByScheduleId(Guid scheduleId);
    }
}
