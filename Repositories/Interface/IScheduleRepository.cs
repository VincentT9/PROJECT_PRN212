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
        public List<Schedule> GetAllSchedules();
        public void CreateSchedule(Schedule schedule);
        public Schedule GetScheduleByScheduleId(Guid scheduleId);
    }
}
