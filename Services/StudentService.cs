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
    public class StudentService : IStudentService
    {
        IStudentRepository _studentRepository;
        public StudentService()
        {
            _studentRepository = new StudentRepository();
        }
        public List<Student> GetAllStudents()
        {
            return _studentRepository.GetAllStudents();
        }
        public List<Student> GetStudentsNotInSchedule(Guid scheduleId)
        {
            return _studentRepository.GetStudentsNotInSchedule(scheduleId);
        }
        public List<Student> GetStudentsByParentId(Guid parentId)
        {
            return _studentRepository.GetStudentsByParentId(parentId);
        }
        public Student GetStudentById(Guid id)
        {
            return _studentRepository.GetStudentById(id);
        }
        
        // Async methods
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetAllStudentsAsync();
        }
        
        public async Task<Student?> GetStudentByIdAsync(Guid studentId)
        {
            return await _studentRepository.GetStudentByIdAsync(studentId);
        }
        
        public async Task<Student?> GetStudentByStudentCodeAsync(string studentCode)
        {
            return await _studentRepository.GetStudentByStudentCodeAsync(studentCode);
        }
        
        public async Task<List<Student>> GetStudentsByParentIdAsync(Guid parentId)
        {
            return await _studentRepository.GetStudentsByParentIdAsync(parentId);
        }
        
        public async Task<List<Student>> GetStudentsByClassAsync(string className)
        {
            return await _studentRepository.GetStudentsByClassAsync(className);
        }
        
        public async Task<List<Student>> GetStudentsBySchoolYearAsync(string schoolYear)
        {
            return await _studentRepository.GetStudentsBySchoolYearAsync(schoolYear);
        }
        
        public async Task CreateStudentAsync(Student student)
        {
            await _studentRepository.CreateStudentAsync(student);
        }
        
        public async Task UpdateStudentAsync(Student student)
        {
            await _studentRepository.UpdateStudentAsync(student);
        }
        
        public async Task DeleteStudentAsync(Guid studentId)
        {
            await _studentRepository.DeleteStudentAsync(studentId);
        }
    }
}
