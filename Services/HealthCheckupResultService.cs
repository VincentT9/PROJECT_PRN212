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
    public class HealthCheckupResultService : IHealthCheckupResultService
    {
        IHealthCheckupResultRepository _repository;
        public HealthCheckupResultService()
        {
            _repository = new HealthCheckupResultRepository();
        }
        public List<HealthCheckupResult> GetByStudentId(Guid studentId)
        {
            return _repository.GetByStudentId(studentId);
        }
        public void Add(HealthCheckupResult result)
        {
            _repository.Add(result);
        }
        public void Update(HealthCheckupResult result)
        {
            _repository.Update(result);
        }
        public void Delete(Guid id)
        {
            _repository.Delete(id);
        }
        public HealthCheckupResult? GetByScheduleDetailId(Guid scheduleDetailId)
        {
            return _repository.GetByScheduleDetailId(scheduleDetailId);
        }
    }
}
