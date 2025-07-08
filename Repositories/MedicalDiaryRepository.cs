
using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;

namespace Repositories
{
    public class MedicalDiaryRepository : IMedicalDiaryRepository
    {
        MedicalDiaryDAO _medicalDiaryDAO = new MedicalDiaryDAO();

        //1. Get all medical diaries
        public List<MedicalDiary> GetMedicalDiaries()
        {
            return _medicalDiaryDAO.GetMedicalDiaries();
        }

        //2. Create a new medical diary
        public void CreateMedicalDiary(MedicalDiary medicalDiary)
        {
            _medicalDiaryDAO.CreateMedicalDiary(medicalDiary);
        }

        //3. Update an existing medical diary
        public void UpdateMedicalDiary(MedicalDiary medicalDiary)
        {
            _medicalDiaryDAO.UpdateMedicalDiary(medicalDiary);
        }

        //4. Delete a medical diary
        public void DeleteMedicalDiary(MedicalDiary medicalDiary)
        {
            _medicalDiaryDAO.DeleteMedicalDiary(medicalDiary);
        }

        //5. Get a medical diary by ID
        public MedicalDiary GetMedicalDiaryById(int id)
        {
            return _medicalDiaryDAO.GetMedicalDiaryById(id);
        }
    }
}
