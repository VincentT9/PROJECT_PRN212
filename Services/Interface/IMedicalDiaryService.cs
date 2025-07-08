using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IMedicalDiaryService
    {
        public List<MedicalDiary> GetMedicalDiaries();
        public void CreateMedicalDiary(MedicalDiary medicalDiary);
        public void UpdateMedicalDiary(MedicalDiary medicalDiary);
        public void DeleteMedicalDiary(MedicalDiary medicalDiary);
        public MedicalDiary GetMedicalDiaryById(int id);
    }
}
