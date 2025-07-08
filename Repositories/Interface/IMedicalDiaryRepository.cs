
using BusinessObjects;

namespace Repositories.Interface
{
    public interface IMedicalDiaryRepository
    {
        public List<MedicalDiary> GetMedicalDiaries();
        public void CreateMedicalDiary(MedicalDiary medicalDiary);
        public void UpdateMedicalDiary(MedicalDiary medicalDiary);
        public void DeleteMedicalDiary(MedicalDiary medicalDiary);
        public MedicalDiary GetMedicalDiaryById(int id);
    }
}
