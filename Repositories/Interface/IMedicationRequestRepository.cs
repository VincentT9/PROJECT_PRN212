using BusinessObjects;

namespace Repositories.Interface
{
    public interface IMedicationRequestRepository
    {
        public List<MedicationRequest> GetMedicationRequests();
        public void CreateMedicationRequest(MedicationRequest medicationRequest);
        public void UpdateMedicationRequest(MedicationRequest medicationRequest);
        public void DeleteMedicationRequest(MedicationRequest medicationRequest);
        public MedicationRequest GetMedicationRequestById(int id);

    }
}
