using BusinessObjects;
using Repositories;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MedicalIncidentService : IMedicalIncidentService
    {
        private readonly IMedicalIncidentRepository _medicalIncidentRepository;

        public MedicalIncidentService()
        {
            _medicalIncidentRepository = new MedicalIncidentRepository();
        }

        //1. Get all medical incidents
        public List<MedicalIncident> GetMedicalIncidents()
        {
            return _medicalIncidentRepository.GetMedicalIncidents();
        }

        //2. Create a new medical incident
        public void CreateMedicalIncident(MedicalIncident medicalIncident)
        {
            _medicalIncidentRepository.CreateMedicalIncident(medicalIncident);
        }

        //3. Update an existing medical incident
        public void UpdateMedicalIncident(MedicalIncident medicalIncident)
        {
            _medicalIncidentRepository.UpdateMedicalIncident(medicalIncident);
        }

        //4. Delete a medical incident
        public void DeleteMedicalIncident(MedicalIncident medicalIncident)
        {
            _medicalIncidentRepository.DeleteMedicalIncident(medicalIncident);
        }

        //5. Get a medical incident by ID
        public MedicalIncident GetMedicalIncidentById(int id)
        {
            return _medicalIncidentRepository.GetMedicalIncidentById(id);
        }
    }
}