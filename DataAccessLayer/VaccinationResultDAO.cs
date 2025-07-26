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
        public List<VaccinationResult> GetAll()
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.VaccinationResults
                .OrderByDescending(v => v.CreateAt)
                .ToList();
        }
        public List<VaccinationResult> GetByStudentId(Guid studentId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.VaccinationResults
                .Include(v => v.ScheduleDetail)
                    .ThenInclude(sd => sd.Student)
                .Where(v => v.ScheduleDetail != null && v.ScheduleDetail.StudentId == studentId)
                .ToList();
        }
    }
}
