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
        public List<Student> GetAllStudents();
        public List<Student> GetStudentsNotInSchedule(Guid scheduleId);
        public List<Student> GetStudentsByParentId(Guid parentId);
        public Student GetStudentById(Guid id);
    }
}
