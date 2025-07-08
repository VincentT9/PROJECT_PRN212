using BusinessObjects;
using Repositories;
using Repositories.Interface;
using Services.Interface;

namespace Services
{
    public class MedicationRequestService : IMedicationRequestService
    {
        private readonly IMedicationRequestRepository _medicationRequestRepository;

        public MedicationRequestService()
        {
            _medicationRequestRepository = new MedicationRequestRepository();
        }
        public List<MedicationRequest> GetMedicationRequests()
        {
            return _medicationRequestRepository.GetMedicationRequests();
        }
        public void CreateMedicationRequest(MedicationRequest medicationRequest)
        {
           _medicationRequestRepository.CreateMedicationRequest(medicationRequest);
        }
        public void UpdateMedicationRequest(MedicationRequest medicationRequest)
        {
            _medicationRequestRepository.UpdateMedicationRequest(medicationRequest);
        }
        public void DeleteMedicationRequest(MedicationRequest medicationRequest)
        {
            _medicationRequestRepository.DeleteMedicationRequest(medicationRequest);
        }
        public MedicationRequest GetMedicationRequestById(int id)
        {
            return _medicationRequestRepository.GetMedicationRequestById(id);
        }

    }
}
