using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class MedicalSupplyDAO
    {
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();

        //1. Get all medical supplies
        public List<MedicalSupply> GetMedicalSupplies()
        {
            return _context.MedicalSupplies.ToList();
        }

        //2.Create a new medical supply
        public void CreateMedicalSupply(MedicalSupply medicalSupply)
        {
            try
            {

                _context.MedicalSupplies.Add(medicalSupply);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //3. Update an existing medical supply
        public void UpdateMedicalSuplly(MedicalSupply medicalSupply)
        {
            try
            {
                _context.Entry<MedicalSupply>(medicalSupply).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //4. Delete a medical supply
        public void DeleteMedicalSupply(MedicalSupply medicalSupply)
        {
            try
            {
                var existMedicalSupply =
                    _context.MedicalSupplies.SingleOrDefault(m => m.Id == medicalSupply.Id);
                if (existMedicalSupply != null)
                {
                    _context.MedicalSupplies.Remove(existMedicalSupply);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //5. Get a medication request by its ID
        public MedicalSupply GetMedicalSupplyById(int id)
        {
            return _context.MedicalSupplies.FirstOrDefault(m => m.Id.Equals(id));
        }
    }
}
