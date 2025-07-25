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
    public class VaccinationResultService : IVaccinationResultService
    {
        IVaccinationResultRepository _vaccinationResultRepository;
        public VaccinationResultService()
        {
            _vaccinationResultRepository = new VaccinationResultRepository();
        }
        public List<VaccinationResult> GetAll()
        {
            return _vaccinationResultRepository.GetAll();
        }
        public List<VaccinationResult> GetByStudentId(Guid studentId)
        {
            return _vaccinationResultRepository.GetByStudentId(studentId);
        }
    }
}
