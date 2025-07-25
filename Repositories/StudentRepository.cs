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
        StudentDAO studentDAO = new StudentDAO();
        public Task CreateStudentAsync(Student student)
        {
            return studentDAO.CreateStudentAsync(student);
        }

        public Task DeleteStudentAsync(Guid studentId)
        {
            return studentDAO.DeleteStudentAsync(studentId);
        }

        public Task<List<Student>> GetAllStudentsAsync()
        {
            return studentDAO.GetAllStudentsAsync();    
        }

        public Task<Student?> GetStudentByIdAsync(Guid studentId)
        {
            return studentDAO.GetStudentByIdAsync(studentId);
        }

        public Task<Student?> GetStudentByStudentCodeAsync(string studentCode)
        {
            return studentDAO.GetStudentByStudentCodeAsync(studentCode);
        }

        public Task<List<Student>> GetStudentsByClassAsync(string className)
        {
            return studentDAO.GetStudentsByClassAsync(className);
        }

        public Task<List<Student>> GetStudentsByParentIdAsync(Guid parentId)
        {
            return studentDAO.GetStudentsByParentIdAsync(parentId);
        }

        public Task<List<Student>> GetStudentsBySchoolYearAsync(string schoolYear)
        {
            return studentDAO.GetStudentsBySchoolYearAsync(schoolYear);
        }

        public Task UpdateStudentAsync(Student student)
        {
            return studentDAO.UpdateStudentAsync(student);
        }
    }
}
