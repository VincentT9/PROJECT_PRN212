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
    public class HealthCheckupResultRepository : IHealthCheckupResultRepository
    {
        HealthCheckupResultDAO dao = new HealthCheckupResultDAO();
        public List<HealthCheckupResult> GetByStudentId(Guid studentId)
        {
            return dao.GetByStudentId(studentId);
        }
        public void Add(HealthCheckupResult result)
        {
            dao.Add(result);
        }
        public void Update(HealthCheckupResult result)
        {
            dao.Update(result);
        }
        public void Delete(Guid id)
        {
            dao.Delete(id);
        }
        public HealthCheckupResult? GetByScheduleDetailId(Guid scheduleDetailId)
        {
            return dao.GetByScheduleDetailId(scheduleDetailId);
        }
    }
}
