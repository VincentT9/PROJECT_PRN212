using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class HealthRecordDAO
    {
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();

        //1. Get all health records
        public List<HealthRecord> GetHealthRecords()
        {
            return _context.HealthRecords.ToList();
        }

        //2.Create a new health record
        public void CreateHealthRecord(HealthRecord healthRecord)
        {
            try
            {
                _context.HealthRecords.Add(healthRecord);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //3. Update an existing health record
        public void UpdateHealthRecord(HealthRecord healthRecord)
        {
            try
            {
                _context.Entry<HealthRecord>(healthRecord).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //4. Delete a health record
        public void DeleteHealthRecord(HealthRecord healthRecord)
        {
            try
            {
                var existHealthRecord =
                    _context.HealthRecords.SingleOrDefault(h => h.Id == healthRecord.Id);
                _context.HealthRecords.Remove(existHealthRecord);

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //5. Get a health record by its ID
        public HealthRecord GetHealthRecordById(int id)
        {
            return _context.HealthRecords.FirstOrDefault(h => h.Id.Equals(id));
        }
    }
}