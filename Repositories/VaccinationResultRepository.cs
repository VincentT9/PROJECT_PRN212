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
    public class VaccinationResultRepository : IVaccinationResultRepository
    {
        VaccinationResultDAO _vaccinationResultDAO = new VaccinationResultDAO();
        public List<VaccinationResult> GetAll()
        {
            return _vaccinationResultDAO.GetAll();
        }
        public List<VaccinationResult> GetByStudentId(Guid studentId)
        {
            return _vaccinationResultDAO.GetByStudentId(studentId);
        }
    }
}
