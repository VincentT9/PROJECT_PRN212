using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IMedicalIncidentService
    {
        public List<MedicalIncident> GetMedicalIncidents();
        public void CreateMedicalIncident(MedicalIncident medicalIncident);
        public void UpdateMedicalIncident(MedicalIncident medicalIncident);
        public void DeleteMedicalIncident(MedicalIncident medicalIncident);
        public MedicalIncident GetMedicalIncidentById(int id);
    }
}