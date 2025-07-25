using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services.Interface
{
    public interface IHealthCheckupResultService
    {
        public List<HealthCheckupResult> GetByStudentId(Guid studentId);
        public void Add(HealthCheckupResult result);
        public void Update(HealthCheckupResult result);
        public void Delete(Guid id);
        public HealthCheckupResult? GetByScheduleDetailId(Guid scheduleDetailId);
    }
}
