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

        public async Task CreateStudentAsync(Student student)
        {
            // Xác thực dữ liệu
            if (student == null)
                throw new ArgumentNullException(nameof(student), "Student không được phép null");

            // Kiểm tra mã học sinh đã tồn tại chưa
            var existingStudent = await _studentRepository.GetStudentByStudentCodeAsync(student.StudentCode);
            if (existingStudent != null)
                throw new InvalidOperationException($"Mã học sinh '{student.StudentCode}' đã tồn tại");

            // Thiết lập thông tin thời gian và ID
            if (student.Id == Guid.Empty)
                student.Id = Guid.NewGuid();

            student.CreateAt = DateTime.Now;
            student.UpdateAt = DateTime.Now;

            await _studentRepository.CreateStudentAsync(student);
        }

        public async Task DeleteStudentAsync(Guid studentId)
        {
            // Kiểm tra học sinh có tồn tại không
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
                throw new KeyNotFoundException($"Không tìm thấy học sinh với ID: {studentId}");

            await _studentRepository.DeleteStudentAsync(studentId);
        }

        public Task<List<Student>> GetAllStudentsAsync()
        {
            return _studentRepository.GetAllStudentsAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(Guid studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
                throw new KeyNotFoundException($"Không tìm thấy học sinh với ID: {studentId}");
                
            return student;
        }

        public Task<Student?> GetStudentByStudentCodeAsync(string studentCode)
        {
            if (string.IsNullOrEmpty(studentCode))
                throw new ArgumentException("Mã học sinh không được để trống", nameof(studentCode));
                
            return _studentRepository.GetStudentByStudentCodeAsync(studentCode);
        }

        public Task<List<Student>> GetStudentsByClassAsync(string className)
        {
            if (string.IsNullOrEmpty(className))
                throw new ArgumentException("Tên lớp không được để trống", nameof(className));
                
            return _studentRepository.GetStudentsByClassAsync(className);
        }

        public Task<List<Student>> GetStudentsByParentIdAsync(Guid parentId)
        {
            if (parentId == Guid.Empty)
                throw new ArgumentException("ID phụ huynh không hợp lệ", nameof(parentId));
                
            return _studentRepository.GetStudentsByParentIdAsync(parentId);
        }

        public Task<List<Student>> GetStudentsBySchoolYearAsync(string schoolYear)
        {
            if (string.IsNullOrEmpty(schoolYear))
                throw new ArgumentException("Năm học không được để trống", nameof(schoolYear));
                
            return _studentRepository.GetStudentsBySchoolYearAsync(schoolYear);
        }

        public async Task UpdateStudentAsync(Student student)
        {
            // Xác thực dữ liệu
            if (student == null)
                throw new ArgumentNullException(nameof(student), "Student không được phép null");
            
            // Kiểm tra học sinh có tồn tại không
            var existingStudent = await _studentRepository.GetStudentByIdAsync(student.Id);
            if (existingStudent == null)
                throw new KeyNotFoundException($"Không tìm thấy học sinh với ID: {student.Id}");
            
            // Cập nhật thông tin thời gian
            student.UpdateAt = DateTime.Now;
            
            await _studentRepository.UpdateStudentAsync(student);
        }
    }
}
