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
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();

        //1. Get all medication requests
        public  List<MedicationRequest> GetMedicationRequests()
        {
            return _context.MedicationRequests.ToList();
        }

        //2.Create a new medication request
        public void CreateMedicationRequest(MedicationRequest medicationRequest)
        {
            try
            {
               
               _context.MedicationRequests.Add(medicationRequest);
               _context.SaveChanges();
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
                _context.Entry<MedicationRequest>(medicationRequest).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //4. Delete a medication request
        public void DeleteMediactionRequest (MedicationRequest medicationRequest)
        {
            try
            {
                var existMedicationRequest =
                    _context.MedicationRequests.SingleOrDefault(m => m.Id == medicationRequest.Id);
                _context.MedicationRequests.Remove(existMedicationRequest);

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //5. Get a medication request by its ID
        public MedicationRequest GetMedicationRequestById(int id)
        {
            return _context.MedicationRequests.FirstOrDefault(m => m.Id.Equals(id));
        }

    }
}
