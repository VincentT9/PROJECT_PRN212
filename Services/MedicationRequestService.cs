using BusinessObjects;
using Repositories;
using Repositories.Interface;
using SchoolMedicalManagementSystem.Enum;
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

        public List<MedicationRequest> GetTodayMedications()
        {
            return _medicationRequestRepository.GetTodayMedications();
        }

        public List<MedicationRequest> GetByStatus(RequestStatus status)
        {
            return _medicationRequestRepository.GetByStatus(status);
        }

        public List<MedicationRequest> GetOverdueOrDone()
        {
            return _medicationRequestRepository.GetOverdueOrDone();
        }

        public List<MedicationRequest> GetByRequestStatusAndDiaries(RequestStatus status)
        {
            return _medicationRequestRepository.GetByRequestStatusAndDiaries(status);
        }

        public MedicationRequest GetMedicationRequestByGuid(Guid id)
        {
            return _medicationRequestRepository.GetMedicationRequestByGuid(id);
        }

        public (int Cancelled, int Overdue, int Completed) GetRequestStats()
        {
            return _medicationRequestRepository.GetRequestStats();
        }
    }
}
