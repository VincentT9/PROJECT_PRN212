using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class MedicalSupplyRepository : IMedicalSupplyRepository
    {
        MedicalSupplyDAO _medicalSupplyDAO = new MedicalSupplyDAO();

        //1. Get all medical supplies
        public List<MedicalSupply> GetMedicalSupplies()
        {
            return _medicalSupplyDAO.GetMedicalSupplies();
        }

        //2. Create a new medical supply
        public void CreateMedicalSupply(MedicalSupply medicalSupply)
        {
            _medicalSupplyDAO.CreateMedicalSupply(medicalSupply);
        }

        //3. Update an existing medical supply
        public void UpdateMedicalSuplly(MedicalSupply medicalSupply)
        {
            _medicalSupplyDAO.UpdateMedicalSuplly(medicalSupply);
        }

        //4. Delete a medical supply
        public void DeleteMedicalSupply(MedicalSupply medicalSupply)
        {
            _medicalSupplyDAO.DeleteMedicalSupply(medicalSupply);
        }

        //5. Get a medical supply by ID
        public MedicalSupply GetMedicalSupplyById(int id)
        {
            return _medicalSupplyDAO.GetMedicalSupplyById(id);
        }
    }
}
