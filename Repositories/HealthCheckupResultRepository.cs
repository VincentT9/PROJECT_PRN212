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
    }
}
