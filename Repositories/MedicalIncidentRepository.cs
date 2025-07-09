using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;

namespace Repositories
{
    public class MedicalIncidentRepository : IMedicalIncidentRepository
    {
        MedicalIncidentDAO _medicalIncidentDAO = new MedicalIncidentDAO();

        //1. Get all medical incidents
        public List<MedicalIncident> GetMedicalIncidents()
        {
            return _medicalIncidentDAO.GetMedicalIncidents();
        }

        //2. Create a new medical incident
        public void CreateMedicalIncident(MedicalIncident medicalIncident)
        {
            _medicalIncidentDAO.CreateMedicalIncident(medicalIncident);
        }

        //3. Update an existing medical incident
        public void UpdateMedicalIncident(MedicalIncident medicalIncident)
        {
            _medicalIncidentDAO.UpdateMedicalIncident(medicalIncident);
        }

        //4. Delete a medical incident
        public void DeleteMedicalIncident(MedicalIncident medicalIncident)
        {
            _medicalIncidentDAO.DeleteMedicalIncident(medicalIncident);
        }

        //5. Get a medical incident by ID
        public MedicalIncident GetMedicalIncidentById(int id)
        {
            return _medicalIncidentDAO.GetMedicalIncidentById(id);
        }
    }
}