using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;
using Repositories.Interface;
using Services.Interface;

namespace Services
{
    public class ScheduleService : IScheduleService
    {
        IScheduleRepository _scheduleRepository;
        public ScheduleService()
        {
            _scheduleRepository = new ScheduleRepository();
        }
        public void CreateSchedule(Schedule schedule)
        {
            _scheduleRepository.CreateSchedule(schedule);
        }

        public List<Schedule> GetAllSchedules()
        {
            return _scheduleRepository.GetAllSchedules();
        }
        public Schedule GetScheduleByScheduleId(Guid scheduleId)
        {
            return _scheduleRepository.GetScheduleByScheduleId(scheduleId);
        }
        public List<Schedule> GetActiveSchedules()
        {
            return _scheduleRepository.GetActiveSchedules();
        }
    }
}
