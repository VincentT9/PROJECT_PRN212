using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories.Interface
{
    public interface IScheduleDetailRepository
    {
        public bool ExistsInSchedule(Guid studentId, Guid scheduleId);
        public void AddScheduleDetail(ScheduleDetail detail);
        public List<Student> GetStudentsByScheduleId(Guid scheduleId);
        public bool IsStudentInSchedule(Guid studentId, Guid scheduleId);
    }
}
