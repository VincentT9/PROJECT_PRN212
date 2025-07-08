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
    public class MedicalSupplyService : IMedicalSupplyService
    {
        private readonly IMedicalSupplyRepository _medicalSupplyRepository;
        public MedicalSupplyService()
        {
            _medicalSupplyRepository = new MedicalSupplyRepository();
        }

        //1. Get all medical supplies
        public List<MedicalSupply> GetMedicalSupplies()
        {
            return _medicalSupplyRepository.GetMedicalSupplies();
        }

        //2. Create a new medical supply
        public void CreateMedicalSupply(MedicalSupply medicalSupply)
        {
            _medicalSupplyRepository.CreateMedicalSupply(medicalSupply);
        }

        //3. Update an existing medical supply
        public void UpdateMedicalSuplly(MedicalSupply medicalSupply)
        {
            _medicalSupplyRepository.UpdateMedicalSuplly(medicalSupply);
        }

        //4. Delete a medical supply
        public void DeleteMedicalSupply(MedicalSupply medicalSupply)
        {
            _medicalSupplyRepository.DeleteMedicalSupply(medicalSupply);
        }

        //5. Get a medical supply by ID
        public MedicalSupply GetMedicalSupplyById(int id)
        {
           return _medicalSupplyRepository.GetMedicalSupplyById(id);
        }
    }
}
