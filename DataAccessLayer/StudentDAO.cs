using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataAccessLayer
{
    public class StudentDAO
    {
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();
        public List<Student> GetAllStudents()
        {
            return _context.Students.ToList();
        }

        public List<Student> GetStudentsNotInSchedule(Guid scheduleId)
        {
            var studentIdsInSchedule = _context.ScheduleDetails
                .Where(sd => sd.ScheduleId == scheduleId && sd.StudentId != null)
                .Select(sd => sd.StudentId!.Value)
                .ToList();

            var students = _context.Students
                .Where(s => !studentIdsInSchedule.Contains(s.Id))
                .ToList();

            return students;
        }
        public List<Student> GetStudentsByParentId(Guid parentId)
        {
            return _context.Students
                .Where(s => s.ParentId == parentId)
                .ToList();
        }

        public Student GetStudentById(Guid id)
        {
            return _context.Students.FirstOrDefault(s => s.Id == id);
        }
    }
}
