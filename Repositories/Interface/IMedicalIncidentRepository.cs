using BusinessObjects;

namespace Repositories.Interface
{
    public interface IMedicalIncidentRepository
    {
        public List<MedicalIncident> GetMedicalIncidents();
        public void CreateMedicalIncident(MedicalIncident medicalIncident);
        public void UpdateMedicalIncident(MedicalIncident medicalIncident);
        public void DeleteMedicalIncident(MedicalIncident medicalIncident);
        public MedicalIncident GetMedicalIncidentById(int id);
    }
}