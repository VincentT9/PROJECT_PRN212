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
        //1. Get all medical diaries
        public List<MedicalDiary> GetMedicalDiaries()
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.MedicalDiaries.ToList();
        }

        //2.Create a new medical diary
        public void CreateMedicalDiary(MedicalDiary medicalDiary)
        {
            try
            {
                using var context = new SwpSchoolMedicalManagementSystemContext();
                context.MedicalDiaries.Add(medicalDiary);
                context.SaveChanges();
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
                using var context = new SwpSchoolMedicalManagementSystemContext();
                context.Entry<MedicalDiary>(medicalDiary).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
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
                using var context = new SwpSchoolMedicalManagementSystemContext();
                var existMedicalDiary =
                    context.MedicalDiaries.SingleOrDefault(m => m.Id == medicalDiary.Id);
                context.MedicalDiaries.Remove(existMedicalDiary);

                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //5. Get a medical diary by its ID
        public MedicalDiary GetMedicalDiaryById(int id)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.MedicalDiaries.FirstOrDefault(m => m.Id.Equals(id));
        }

        public List<MedicalDiary> GetByStatus(int status)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.MedicalDiaries.Where(m => m.Status == status).ToList();
        }
    }
}
