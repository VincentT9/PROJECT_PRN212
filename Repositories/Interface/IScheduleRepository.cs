<<<<<<< HEAD
﻿using BusinessObjects;
using System;
=======
﻿using System;
>>>>>>> origin/quocbao
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
=======
using BusinessObjects;
>>>>>>> origin/quocbao

namespace Repositories.Interface
{
    public interface IScheduleRepository
    {
<<<<<<< HEAD
        public List<Schedule> GetSchedules();
        public void CreateSchedule(Schedule schedule);
        public void UpdateSchedule(Schedule schedule);
        public void DeleteSchedule(Schedule schedule);
        public List<ScheduleDetail> GetScheduleDetailsByScheduleId(Guid scheduleId);
        public List<Guid> GetStudentIdsByScheduleId(Guid scheduleId);
=======
        public List<Schedule> GetAllSchedules();
        public void CreateSchedule(Schedule schedule);
        public Schedule GetScheduleByScheduleId(Guid scheduleId);
        public List<Schedule> GetActiveSchedules();
>>>>>>> origin/quocbao
    }
}
