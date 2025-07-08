using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class MedicalDiaryDAO
    {
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();

        //1. Get all medical diaries
        public List<MedicalDiary> GetMedicalDiaries()
        {
            return _context.MedicalDiaries.ToList();
        }

        //2.Create a new medical diary
        public void CreateMedicalDiary(MedicalDiary medicalDiary)
        {
            try
            {

                _context.MedicalDiaries.Add(medicalDiary);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //3. Update an existing medical diary
        public void UpdateMedicalDiary(MedicalDiary medicalDiary)
        {
            try
            {
                _context.Entry<MedicalDiary>(medicalDiary).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //4. Delete a medical diary
        public void DeleteMedicalDiary(MedicalDiary medicalDiary)
        {
            try
            {
                var existMedicalDiary =
                    _context.MedicalDiaries.SingleOrDefault(m => m.Id == medicalDiary.Id);
                _context.MedicalDiaries.Remove(existMedicalDiary);

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //5. Get a medical diary by its ID
        public MedicalDiary GetMedicalDiaryById(int id)
        {
            return _context.MedicalDiaries.FirstOrDefault(m => m.Id.Equals(id));
        }
    }
}
