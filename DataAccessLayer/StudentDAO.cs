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


        public Student GetStudentByStudentCode(string studentCode)
        {
            return _context.Students.FirstOrDefault(s => s.StudentCode == studentCode);
        }

        public List<Student> GetStudentsByClass(string className)
        {
            return _context.Students
                .Where(s => s.Class == className)
                .ToList();
        }

        public List<Student> GetStudentsBySchoolYear(string schoolYear)
        {
            return _context.Students
                .Where(s => s.SchoolYear == schoolYear)
                .ToList();
        }

        public void CreateStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public void UpdateStudent(Student student)
        {
            var existingStudent = _context.Students.FirstOrDefault(s => s.Id == student.Id);
            if (existingStudent != null)
            {
                _context.Entry(existingStudent).CurrentValues.SetValues(student);
                _context.SaveChanges();
            }
        }

        public void DeleteStudent(Guid studentId)
        {
            var student = _context.Students.FirstOrDefault(s => s.Id == studentId);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
        }
    }
}
