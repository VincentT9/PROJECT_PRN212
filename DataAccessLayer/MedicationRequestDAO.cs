using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalManagementSystem.Enum;
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
        public List<MedicationRequest> GetTodayMedications()
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            DateTime today = DateTime.UtcNow;
            return context.MedicationRequests.Include(r => r.Student)
                .Where(r => r.StartDate <= today && r.EndDate >= today)
                .ToList();
        }

        public List<MedicationRequest> GetByStatus(RequestStatus status)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.MedicationRequests.Include(r => r.Student)
                .Where(r => r.Status == status)
                .ToList();
        }        
        
        public List<MedicationRequest> GetByRequestStatusAndDiaries(RequestStatus status)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.MedicationRequests
                    .Where(mr => mr.Status == RequestStatus.Received)
                    .Where(mr => !context.MedicalDiaries.Any(md => md.MedicationReqId == mr.Id))
                    .Include(mr => mr.Student)
                    .ToList();
        }

        public List<MedicationRequest> GetOverdueOrDone()
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.MedicationRequests
                    .Where(mr => mr.Status == RequestStatus.Received)
                    .Where(mr => mr.MedicalDiaries.Any(md => md.Status == (int)MedicationStatus.Taken || 
                                                             md.Status == (int)MedicationStatus.Missed))
                    .Include(mr => mr.Student)
                    .ToList();
        }

        public MedicationRequest GetMedicationRequestByGuid(Guid id)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.MedicationRequests.FirstOrDefault(m => m.Id == id);
        }

        public (int Cancelled, int Overdue, int Completed) GetRequestStats()
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();

            int cancelled = context.MedicationRequests
                .Count(mr => mr.MedicalDiaries.Any(md => md.Status == (int)MedicationStatus.NotTaken));

            int overdue = context.MedicationRequests
                .Where(mr => mr.Status == RequestStatus.Received)
                .Count(mr => mr.MedicalDiaries.Any(md => md.Status == (int)MedicationStatus.Missed));

            int completed = context.MedicationRequests
                .Where(mr => mr.Status == RequestStatus.Received)
                .Count(mr => mr.MedicalDiaries.Any(md => md.Status == (int)MedicationStatus.Taken));

            return (cancelled, overdue, completed);
        }
    }
}
