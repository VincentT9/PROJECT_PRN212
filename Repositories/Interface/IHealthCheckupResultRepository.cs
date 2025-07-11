using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories.Interface
{
    public interface IHealthCheckupResultRepository
    {
        public List<HealthCheckupResult> GetByStudentId(Guid studentId);
    }
}
