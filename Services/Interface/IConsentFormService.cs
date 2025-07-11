using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services.Interface
{
    public interface IConsentFormService
    {
        public bool IsStudentApprovedByParent(Guid studentId, Guid campaignId);
        public ConsentForm IsExists(Guid studentId, Guid campaignId);
        public bool AddNewConsentForm(ConsentForm form);
        public bool UpdateConsentForm(ConsentForm form);
    }
}
