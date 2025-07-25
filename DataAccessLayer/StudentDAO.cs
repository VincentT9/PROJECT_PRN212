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
        public List<Student> GetAllStudents()
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.Students.ToList();
        }

        public List<Student> GetStudentsNotInSchedule(Guid scheduleId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            var studentIdsInSchedule = context.ScheduleDetails
                .Where(sd => sd.ScheduleId == scheduleId)
                .Select(sd => sd.StudentId)
                .ToList();

            var students = context.Students
                .Where(s => !studentIdsInSchedule.Contains(s.Id))
                .ToList();

            return students;
        }

        public List<Student> GetStudentsByParentId(Guid parentId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.Students
                .Where(s => s.ParentId == parentId)
                .ToList();
        }

        public Student GetStudentById(Guid id)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.Students.FirstOrDefault(s => s.Id == id);
        }

        public Student GetStudentByStudentCode(string studentCode)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.Students.FirstOrDefault(s => s.StudentCode == studentCode);
        }

        public List<Student> GetStudentsByClass(string className)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.Students
                .Where(s => s.Class == className)
                .ToList();
        }

        public List<Student> GetStudentsBySchoolYear(string schoolYear)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.Students
                .Where(s => s.SchoolYear == schoolYear)
                .ToList();
        }

        public void CreateStudent(Student student)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            context.Students.Add(student);
            context.SaveChanges();
        }

        public void UpdateStudent(Student student)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            var existingStudent = context.Students.FirstOrDefault(s => s.Id == student.Id);
            if (existingStudent != null)
            {
                context.Entry(existingStudent).CurrentValues.SetValues(student);
                context.SaveChanges();
            }
        }

        public void DeleteStudent(Guid studentId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            var student = context.Students.FirstOrDefault(s => s.Id == studentId);
            if (student != null)
            {
                context.Students.Remove(student);
                context.SaveChanges();
            }
        }
    }
}
