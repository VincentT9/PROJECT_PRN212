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
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();

        //1. Get all students
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Include(s => s.HealthRecord)
                .ToListAsync();
        }

        //2. Get student by ID
        public async Task<Student?> GetStudentByIdAsync(Guid studentId)
        {
            return await _context.Students
                .Include(s => s.HealthRecord)
                .FirstOrDefaultAsync(s => s.Id == studentId);
        }

        //3. Get student by Student Code
        public async Task<Student?> GetStudentByStudentCodeAsync(string studentCode)
        {
            return await _context.Students
                .Include(s => s.HealthRecord)
                .FirstOrDefaultAsync(s => s.StudentCode == studentCode);
        }

        //4. Get students by Parent ID
        public async Task<List<Student>> GetStudentsByParentIdAsync(Guid parentId)
        {
            return await _context.Students
                .Include(s => s.HealthRecord)
                .Where(s => s.ParentId == parentId)
                .ToListAsync();
        }

        //5. Get students by Class
        public async Task<List<Student>> GetStudentsByClassAsync(string className)
        {
            return await _context.Students
                .Include(s => s.HealthRecord)
                .Where(s => s.Class == className)
                .ToListAsync();
        }

        //6. Get students by School Year
        public async Task<List<Student>> GetStudentsBySchoolYearAsync(string schoolYear)
        {
            return await _context.Students
                .Include(s => s.HealthRecord)
                .Where(s => s.SchoolYear == schoolYear)
                .ToListAsync();
        }
        //7. Create a new student
        public async Task CreateStudentAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        //8. Update an existing student
        public async Task UpdateStudentAsync(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        //9. Delete a student
        public async Task DeleteStudentAsync(Guid studentId)
        {
            var student = await GetStudentByIdAsync(studentId);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }

        }
    }
}
