using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class MedicalIncidentDAO
    {
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();

        //1. Get all medical incidents
        public List<MedicalIncident> GetMedicalIncidents()
        {
            return _context.MedicalIncidents.ToList();
        }

        //2.Create a new medical incident
        public void CreateMedicalIncident(MedicalIncident medicalIncident)
        {
            try
            {
                _context.MedicalIncidents.Add(medicalIncident);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //3. Update an existing medical incident
        public void UpdateMedicalIncident(MedicalIncident medicalIncident)
        {
            try
            {
                _context.Entry<MedicalIncident>(medicalIncident).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //4. Delete a medical incident
        public void DeleteMedicalIncident(MedicalIncident medicalIncident)
        {
            try
            {
                var existMedicalIncident =
                    _context.MedicalIncidents.SingleOrDefault(m => m.Id == medicalIncident.Id);
                _context.MedicalIncidents.Remove(existMedicalIncident);

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //5. Get a medical incident by its ID
        public MedicalIncident GetMedicalIncidentById(int id)
        {
            return _context.MedicalIncidents.FirstOrDefault(m => m.Id.Equals(id));
        }
    }
}