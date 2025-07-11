using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class VaccinationResultDAO
    {
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();
        public List<VaccinationResult> GetAll()
        {
            return _context.VaccinationResults
                .OrderByDescending(v => v.CreateAt)
                .ToList();
        }
        public List<VaccinationResult> GetByStudentId(Guid studentId)
        {
            return _context.VaccinationResults
                .Include(v => v.ScheduleDetail)
                    .ThenInclude(sd => sd.Student)
                .Where(v => v.ScheduleDetail != null && v.ScheduleDetail.StudentId == studentId)
                .ToList();
        }
    }
}
