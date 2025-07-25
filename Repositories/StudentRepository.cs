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
    public class StudentRepository : IStudentRepository
    {
        StudentDAO dao = new StudentDAO();
        
        // Async methods
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await Task.FromResult(dao.GetAllStudents());
        }
        
        public async Task<Student?> GetStudentByIdAsync(Guid studentId)
        {
            return await Task.FromResult(dao.GetStudentById(studentId));
        }
        
        public async Task<Student?> GetStudentByStudentCodeAsync(string studentCode)
        {
            return await Task.FromResult(dao.GetStudentByStudentCode(studentCode));
        }
        
        public async Task<List<Student>> GetStudentsByParentIdAsync(Guid parentId)
        {
            return await Task.FromResult(dao.GetStudentsByParentId(parentId));
        }
        
        public async Task<List<Student>> GetStudentsByClassAsync(string className)
        {
            return await Task.FromResult(dao.GetStudentsByClass(className));
        }
        
        public async Task<List<Student>> GetStudentsBySchoolYearAsync(string schoolYear)
        {
            return await Task.FromResult(dao.GetStudentsBySchoolYear(schoolYear));
        }
        
        public async Task CreateStudentAsync(Student student)
        {
            await Task.Run(() => dao.CreateStudent(student));
        }
        
        public async Task UpdateStudentAsync(Student student)
        {
            await Task.Run(() => dao.UpdateStudent(student));
        }
        
        public async Task DeleteStudentAsync(Guid studentId)
        {
            await Task.Run(() => dao.DeleteStudent(studentId));
        }
        
        // Sync methods
        public List<Student> GetAllStudents()
        {
            return dao.GetAllStudents();
        }
        
        public List<Student> GetStudentsNotInSchedule(Guid scheduleId)
        {
            return dao.GetStudentsNotInSchedule(scheduleId);
        }
        
        public List<Student> GetStudentsByParentId(Guid parentId)
        {
            return dao.GetStudentsByParentId(parentId);
        }
        
        public Student GetStudentById(Guid id)
        {
            return dao.GetStudentById(id);
        }
    }
}
