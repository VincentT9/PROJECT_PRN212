using BusinessObjects;
using SchoolMedicalManagementSystem.Enum;

namespace Repositories.Interface
{
    public interface IMedicationRequestRepository
    {
        public List<MedicationRequest> GetMedicationRequests();
        public void CreateMedicationRequest(MedicationRequest medicationRequest);
        public void UpdateMedicationRequest(MedicationRequest medicationRequest);
        public void DeleteMedicationRequest(MedicationRequest medicationRequest);
        public MedicationRequest GetMedicationRequestById(int id);
        public List<MedicationRequest> GetTodayMedications();
        public List<MedicationRequest> GetByStatus(RequestStatus status);
        public List<MedicationRequest> GetOverdueOrDone();
        public List<MedicationRequest> GetByRequestStatusAndDiaries(RequestStatus status);
        public MedicationRequest GetMedicationRequestByGuid(Guid id);
        public (int Cancelled, int Overdue, int Completed) GetRequestStats();
    }
}
