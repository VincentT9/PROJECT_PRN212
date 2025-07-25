using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class MedicationRequestDAO
    {
        //1. Get all medication requests
        public List<MedicationRequest> GetMedicationRequests()
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.MedicationRequests.ToList();
        }

        //2.Create a new medication request
        public void CreateMedicationRequest(MedicationRequest medicationRequest)
        {
            try
            {
                using var context = new SwpSchoolMedicalManagementSystemContext();
                context.MedicationRequests.Add(medicationRequest);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //3. Update an existing medication request
        public void UpdateMedicationRequest(MedicationRequest medicationRequest)
        {
            try
            {
                using var context = new SwpSchoolMedicalManagementSystemContext();
                context.Entry<MedicationRequest>(medicationRequest).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //4. Delete a medication request
        public void DeleteMediactionRequest(MedicationRequest medicationRequest)
        {
            try
            {
                using var context = new SwpSchoolMedicalManagementSystemContext();
                var existMedicationRequest =
                    context.MedicationRequests.SingleOrDefault(m => m.Id == medicationRequest.Id);
                context.MedicationRequests.Remove(existMedicationRequest);

                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //5. Get a medication request by its ID
        public MedicationRequest GetMedicationRequestById(int id)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.MedicationRequests.FirstOrDefault(m => m.Id.Equals(id));
        }

    }
}
