using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories.Interface
{
    public interface IStudentRepository
    {
        public Task<List<Student>> GetAllStudentsAsync();
        public Task<Student?> GetStudentByIdAsync(Guid studentId);
        public Task<Student?> GetStudentByStudentCodeAsync(string studentCode);
        public Task<List<Student>> GetStudentsByParentIdAsync(Guid parentId);
        public Task<List<Student>> GetStudentsByClassAsync(string className);
        public Task<List<Student>> GetStudentsBySchoolYearAsync(string schoolYear);
        public Task CreateStudentAsync(Student student);
        public Task UpdateStudentAsync(Student student);
        public Task DeleteStudentAsync(Guid studentId);
        public List<Student> GetAllStudents();
        public List<Student> GetStudentsNotInSchedule(Guid scheduleId);
        public List<Student> GetStudentsByParentId(Guid parentId);
        public Student GetStudentById(Guid id);
    }
}
