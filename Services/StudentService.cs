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

        public Task CreateStudentAsync(Student student)
        {
            return _studentRepository.CreateStudentAsync(student);
        }

        public Task DeleteStudentAsync(Guid studentId)
        {
            return _studentRepository.DeleteStudentAsync(studentId);
        }

        public Task<List<Student>> GetAllStudentsAsync()
        {
            return _studentRepository.GetAllStudentsAsync();
        }

        public Task<Student?> GetStudentByIdAsync(Guid studentId)
        {
            return _studentRepository.GetStudentByIdAsync(studentId);   
        }

        public Task<Student?> GetStudentByStudentCodeAsync(string studentCode)
        {
            return _studentRepository.GetStudentByStudentCodeAsync(studentCode);
        }

        public Task<List<Student>> GetStudentsByClassAsync(string className)
        {
            return _studentRepository.GetStudentsByClassAsync(className);
        }

        public Task<List<Student>> GetStudentsByParentIdAsync(Guid parentId)
        {
            return _studentRepository.GetStudentsByParentIdAsync(parentId);
        }

        public Task<List<Student>> GetStudentsBySchoolYearAsync(string schoolYear)
        {
            return _studentRepository.GetStudentsBySchoolYearAsync(schoolYear);
        }

        public Task UpdateStudentAsync(Student student)
        {
            return _studentRepository.UpdateStudentAsync(student);
        }
    }
}
