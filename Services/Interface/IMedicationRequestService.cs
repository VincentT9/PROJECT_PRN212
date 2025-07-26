using BusinessObjects;
using SchoolMedicalManagementSystem.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IMedicationRequestService
    {
        public List<MedicationRequest> GetMedicationRequests();
        public void CreateMedicationRequest(MedicationRequest medicationRequest);
        public void UpdateMedicationRequest(MedicationRequest medicationRequest);
        public void DeleteMedicationRequest(MedicationRequest medicationRequest);
        public List<MedicationRequest> GetTodayMedications();
        public List<MedicationRequest> GetByStatus(RequestStatus status);
        public List<MedicationRequest> GetOverdueOrDone();
        public MedicationRequest GetMedicationRequestById(int id);
    }
}
