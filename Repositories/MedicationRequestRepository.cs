using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;
using SchoolMedicalManagementSystem.Enum;

namespace Repositories
{
    public class MedicationRequestRepository : IMedicationRequestRepository
    {
        MedicationRequestDAO medicationRequestDAO = new MedicationRequestDAO();

        public List<MedicationRequest> GetMedicationRequests()
        {
            return medicationRequestDAO.GetMedicationRequests();
        }

        public void CreateMedicationRequest(MedicationRequest medicationRequest)
        {
            medicationRequestDAO.CreateMedicationRequest(medicationRequest);
        }

        public void UpdateMedicationRequest(MedicationRequest medicationRequest)
        {
            medicationRequestDAO.UpdateMedicationRequest(medicationRequest);
        }

        public void DeleteMedicationRequest(MedicationRequest medicationRequest)
        {
            medicationRequestDAO.DeleteMediactionRequest(medicationRequest);
        }

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

        public List<MedicationRequest> GetByRequestStatusAndDiaries(RequestStatus status)
        {
            return medicationRequestDAO.GetByRequestStatusAndDiaries(status);
        }

        public MedicationRequest GetMedicationRequestByGuid(Guid id)
        {
            return medicationRequestDAO.GetMedicationRequestByGuid(id);
        }

        public (int Cancelled, int Overdue, int Completed) GetRequestStats()
        {
            return medicationRequestDAO.GetRequestStats();
        }
    }
}
