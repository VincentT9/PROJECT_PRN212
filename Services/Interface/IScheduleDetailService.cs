using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services.Interface
{
    public interface IScheduleDetailService
    {
        public bool ExistsInSchedule(Guid studentId, Guid scheduleId);
        public bool AddScheduleDetail(Guid studentId, Guid scheduleId);
        public List<Student> GetStudentsByScheduleId(Guid scheduleId);
        public bool IsStudentInSchedule(Guid studentId, Guid scheduleId);
    }
}
