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
        public bool IsStudentApprovedByParent(Guid studentId, Guid campaignId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.ConsentForms
                .Any(cf => cf.StudentId == studentId
                        && cf.CampaignId == campaignId
                        && cf.IsApproved == true);
        }
        public ConsentForm IsExists(Guid studentId, Guid campaignId)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.ConsentForms.FirstOrDefault(cf => cf.StudentId == studentId && cf.CampaignId == campaignId);
        }

        public void AddNewConsentForm(ConsentForm form)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            context.ConsentForms.Add(form);
            context.SaveChanges();
        }
        public void UpdateConsentForm(ConsentForm form)
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            context.ConsentForms.Update(form);
            context.SaveChanges();
        }
    }
}
