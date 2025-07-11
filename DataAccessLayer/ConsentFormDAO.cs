using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class ConsentFormDAO
    {
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();
        public bool IsStudentApprovedByParent(Guid studentId, Guid campaignId)
        {
            return _context.ConsentForms
                .Any(cf => cf.StudentId == studentId
                        && cf.CampaignId == campaignId
                        && cf.IsApproved == true);
        }
        public ConsentForm IsExists(Guid studentId, Guid campaignId)
        {
            return _context.ConsentForms.FirstOrDefault(cf => cf.StudentId == studentId && cf.CampaignId == campaignId);
        }

        public void AddNewConsentForm(ConsentForm form)
        {
            _context.ConsentForms.Add(form);
            _context.SaveChanges();
        }
        public void UpdateConsentForm(ConsentForm form)
        {
            _context.ConsentForms.Update(form);
            _context.SaveChanges();
        }
    }
}
