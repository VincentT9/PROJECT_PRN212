using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;
using Repositories.Interface;
using Services.Interface;

namespace Services
{
    public class ConsentFormService : IConsentFormService
    {
        IConsentFormRepository _consentFormRepository;
        public ConsentFormService()
        {
            _consentFormRepository = new ConsentFormRepository();
        }
        public bool IsStudentApprovedByParent(Guid studentId, Guid campaignId)
        {
            return _consentFormRepository.IsStudentApprovedByParent(studentId, campaignId);
        }
        public ConsentForm IsExists(Guid studentId, Guid campaignId)
        {
            return _consentFormRepository.IsExists(studentId, campaignId);
        }
        public bool AddNewConsentForm(ConsentForm form)
        {
            if(form == null)
            {
                return false;
            }
            _consentFormRepository.AddNewConsentForm(form);
            return true;
        }
        public bool UpdateConsentForm(ConsentForm form)
        {
            if (form == null)
            {
                return false;
            }
            _consentFormRepository.UpdateConsentForm(form);
            return true;
        }
    }
}
