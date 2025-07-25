using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;

namespace Repositories
{
    public class ConsentFormRepository : IConsentFormRepository
    {
        ConsentFormDAO dao = new ConsentFormDAO();
        public bool IsStudentApprovedByParent(Guid studentId, Guid campaignId)
        {
            return dao.IsStudentApprovedByParent(studentId, campaignId);
        }
        public ConsentForm IsExists(Guid studentId, Guid campaignId)
        {
            return dao.IsExists(studentId, campaignId);
        }
        public void AddNewConsentForm(ConsentForm form)
        {
            dao.AddNewConsentForm(form);
        }
        public void UpdateConsentForm(ConsentForm form)
        {
            dao.UpdateConsentForm(form);
        }
    }
}
