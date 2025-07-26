using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;
using SchoolMedicalManagementSystem.Enum;

namespace Repositories
{
    public class MedicationRequestRepository : IMedicationRequestRepository
    {
        MedicationRequestDAO medicationRequestDAO = new MedicationRequestDAO();

        //1. Get all medication requests
        public List<MedicationRequest> GetMedicationRequests()
        {
            return medicationRequestDAO.GetMedicationRequests();
        }

        //2.Create a new medication request
        public void CreateMedicationRequest(MedicationRequest medicationRequest)
        {
            medicationRequestDAO.CreateMedicationRequest(medicationRequest);
        }

        //3. Update an existing medication request
        public void UpdateMedicationRequest(MedicationRequest medicationRequest)
        {
            medicationRequestDAO.UpdateMedicationRequest(medicationRequest);
        }

        //4. Delete a medication request
        public void DeleteMedicationRequest(MedicationRequest medicationRequest)
        {
            medicationRequestDAO.DeleteMediactionRequest(medicationRequest);
        }

        //5. Get a medication request by ID
        public MedicationRequest GetMedicationRequestById(int id)
        {
            return medicationRequestDAO.GetMedicationRequestById(id);
        }

        public List<MedicationRequest> GetTodayMedications()
        {
            return medicationRequestDAO.GetTodayMedications();
        }

        public List<MedicationRequest> GetByStatus(RequestStatus status)
        {
            return medicationRequestDAO.GetByStatus(status);
        }

        public List<MedicationRequest> GetOverdueOrDone()
        {
            return medicationRequestDAO.GetOverdueOrDone();
        }
    }
}
