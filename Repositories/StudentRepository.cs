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
