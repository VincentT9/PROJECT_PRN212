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
    }
}
