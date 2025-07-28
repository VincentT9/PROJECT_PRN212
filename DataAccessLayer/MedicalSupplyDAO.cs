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
        //1. Get all medical supplies
        public List<MedicalSupply> GetMedicalSupplies()
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.MedicalSupplies.ToList();
        }

        //2.Create a new medical supply
        public void CreateMedicalSupply(MedicalSupply medicalSupply)
        {
            try
            {
                using var context = new SwpSchoolMedicalManagementSystemContext();
                context.MedicalSupplies.Add(medicalSupply);
                context.SaveChanges();
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
                using var context = new SwpSchoolMedicalManagementSystemContext();
                var exit = context.MedicalSupplies.SingleOrDefault(m => m.Id == medicalSupply.Id);
                if (exit == null)
                {
                    throw new Exception("Medical supply not found");
                }
                exit.SupplyName = medicalSupply.SupplyName;
                exit.SupplyType = medicalSupply.SupplyType;
                exit.Quantity = medicalSupply.Quantity;
                exit.Unit = medicalSupply.Unit;
                exit.Quantity = medicalSupply.Quantity;
                exit.Supplier = medicalSupply.Supplier;
                exit.Image = medicalSupply.Image;
                exit.CreatedBy = medicalSupply.CreatedBy;
                exit.UpdatedBy = medicalSupply.UpdatedBy;
                exit.UpdateAt = DateTime.UtcNow;
                exit.CreateAt = medicalSupply.CreateAt;
                context.SaveChanges();
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
                using var context = new SwpSchoolMedicalManagementSystemContext();
                var existMedicalSupply =
                    context.MedicalSupplies.SingleOrDefault(m => m.Id == medicalSupply.Id);
                if (existMedicalSupply != null)
                {
                    context.MedicalSupplies.Remove(existMedicalSupply);
                    context.SaveChanges();
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
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.MedicalSupplies.FirstOrDefault(m => m.Id.Equals(id));
        }
    }
}
