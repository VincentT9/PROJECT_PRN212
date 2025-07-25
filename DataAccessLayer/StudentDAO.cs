using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class StudentDAO
    {
        public List<Student> GetAllStudents()
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                return _context.Students
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all students: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public List<Student> GetStudentsNotInSchedule(Guid scheduleId)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var studentIdsInSchedule = _context.ScheduleDetails
                    .Where(sd => sd.ScheduleId == scheduleId && sd.StudentId != null)
                    .Select(sd => sd.StudentId!.Value)
                    .ToList();

                var students = _context.Students
                    .Where(s => !studentIdsInSchedule.Contains(s.Id))
                    .AsNoTracking()
                    .ToList();

                return students;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting students not in schedule: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }
        
        public List<Student> GetStudentsByParentId(Guid parentId)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                return _context.Students
                    .Where(s => s.ParentId == parentId)
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting students by parent ID: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public Student GetStudentById(Guid id)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                return _context.Students
                    .AsNoTracking()
                    .FirstOrDefault(s => s.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting student by ID: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }
        
        public Student GetStudentByStudentCode(string studentCode)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                return _context.Students
                    .AsNoTracking()
                    .FirstOrDefault(s => s.StudentCode == studentCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting student by student code: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }
        
        public List<Student> GetStudentsByClass(string className)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                return _context.Students
                    .Where(s => s.Class == className)
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting students by class: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }
        
        public List<Student> GetStudentsBySchoolYear(string schoolYear)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                return _context.Students
                    .Where(s => s.SchoolYear == schoolYear)
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting students by school year: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }
        
        public void CreateStudent(Student student)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                
                // Ensure ID is set
                if (student.Id == Guid.Empty)
                {
                    student.Id = Guid.NewGuid();
                }
                
                // Set creation and update timestamps
                if (student.CreateAt == default)
                {
                    student.CreateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                }
                else
                {
                    student.CreateAt = DateTime.SpecifyKind(student.CreateAt, DateTimeKind.Utc);
                }
                
                student.UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                
                // Convert DateOfBirth to UTC for PostgreSQL if it has a value
                if (student.DateOfBirth != default)
                {
                    student.DateOfBirth = DateTime.SpecifyKind(student.DateOfBirth, DateTimeKind.Utc);
                }
                
                _context.Students.Add(student);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating student: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }
        
        public void UpdateStudent(Student student)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var existingStudent = _context.Students.Find(student.Id);
                if (existingStudent != null)
                {
                    // Update individual properties
                    existingStudent.StudentCode = student.StudentCode;
                    existingStudent.FullName = student.FullName;
                    
                    // Convert DateOfBirth to UTC
                    if (student.DateOfBirth != default)
                    {
                        existingStudent.DateOfBirth = DateTime.SpecifyKind(student.DateOfBirth, DateTimeKind.Utc);
                    }
                    
                    existingStudent.Gender = student.Gender;
                    existingStudent.Class = student.Class;
                    existingStudent.SchoolYear = student.SchoolYear;
                    existingStudent.ParentId = student.ParentId;
                    existingStudent.UpdatedBy = student.UpdatedBy;
                    existingStudent.UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating student: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }
        
        public void DeleteStudent(Guid studentId)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var student = _context.Students.Find(studentId);
                if (student != null)
                {
                    _context.Students.Remove(student);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting student: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }
    }
}
