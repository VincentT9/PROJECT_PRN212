using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories.Interface
{
    public interface IConsentFormRepository
    {
        public bool IsStudentApprovedByParent(Guid studentId, Guid campaignId);
        public ConsentForm IsExists(Guid studentId, Guid campaignId);
        public void AddNewConsentForm(ConsentForm form);
        public void UpdateConsentForm(ConsentForm form);
    }
}
