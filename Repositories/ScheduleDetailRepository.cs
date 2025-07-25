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
    public class ScheduleDetailRepository : IScheduleDetailRepository
    {
        ScheduleDetailDAO dao = new ScheduleDetailDAO();
        public void AddScheduleDetail(ScheduleDetail detail)
        {
            dao.AddScheduleDetail(detail);
        }

        public bool ExistsInSchedule(Guid studentId, Guid scheduleId)
        {
            return dao.ExistsInSchedule(studentId, scheduleId);
        }
        public List<Student> GetStudentsByScheduleId(Guid scheduleId)
        {
            return dao.GetStudentsByScheduleId(scheduleId);
        }
        public bool IsStudentInSchedule(Guid studentId, Guid scheduleId)
        {
            return dao.IsStudentInSchedule(studentId, scheduleId);
        }
        public List<ScheduleDetail> GetByScheduleId(Guid scheduleId)
        {
            return dao.GetByScheduleId(scheduleId);
        }
        public ScheduleDetail? GetByStudentAndSchedule(Guid studentId, Guid scheduleId)
        {
            return dao.GetByStudentAndSchedule(studentId, scheduleId);
        }
    }
}
