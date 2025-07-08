using BusinessObjects;
using Repositories;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MedicalDiaryService : IMedicalDiaryService
    {
        private readonly IMedicalDiaryRepository _medicalDiaryRepository;

        public MedicalDiaryService()
        {
            _medicalDiaryRepository = new MedicalDiaryRepository();
        }

        //1. Get all medical diaries
        public List<MedicalDiary> GetMedicalDiaries()
        {
           return _medicalDiaryRepository.GetMedicalDiaries();
        }

        //2. Create a new medical diary
        public void CreateMedicalDiary(MedicalDiary medicalDiary)
        {
           _medicalDiaryRepository.CreateMedicalDiary(medicalDiary);
        }


        //3. Update an existing medical diary
        public void UpdateMedicalDiary(MedicalDiary medicalDiary)
        {
            _medicalDiaryRepository.UpdateMedicalDiary(medicalDiary);
        }

        //4. Delete a medical diary
        public void DeleteMedicalDiary(MedicalDiary medicalDiary)
        {
            _medicalDiaryRepository.DeleteMedicalDiary(medicalDiary);   
        }

        //5. Get a medical diary by ID
        public MedicalDiary GetMedicalDiaryById(int id)
        {
            return _medicalDiaryRepository.GetMedicalDiaryById(id);
        }
    }
}
