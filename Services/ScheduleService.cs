using BusinessObjects;
using Repositories;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ScheduleService : IScheduleService
    {
        ScheduleRepository scheduleRepository = new ScheduleRepository();
        public void CreateSchedule(Schedule schedule) => scheduleRepository.CreateSchedule(schedule);   


        public void DeleteSchedule(Schedule schedule) => scheduleRepository.DeleteSchedule(schedule);   


        public List<ScheduleDetail> GetScheduleDetailsByScheduleId(Guid scheduleId) => scheduleRepository.GetScheduleDetailsByScheduleId(scheduleId);   


        public List<Schedule> GetSchedules() => scheduleRepository.GetSchedules();  

        public List<Guid> GetStudentIdsByScheduleId(Guid scheduleId) => scheduleRepository.GetStudentIdsByScheduleId(scheduleId);   


        public void UpdateSchedule(Schedule schedule) => scheduleRepository.UpdateSchedule(schedule);   

    }
}
