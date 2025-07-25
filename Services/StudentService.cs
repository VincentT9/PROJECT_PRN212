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
    }
}
