using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using SchoolMedicalManagementSystem.Enum;

namespace DataAccessLayer
{
    public class CampaignDAO
    {
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();
        public List<Campaign> GetAllCampaigns()
        {
            return _context.Campaigns.
                Where(c => c.Status == 
                CampaignStatus.InProgress).ToList();
        }

        public void CreateCampaign(Campaign campaign)
        {
            var existingCampaign = _context.Campaigns.FirstOrDefault(c => c.Id == campaign.Id);
            if(existingCampaign != null)
            {
                return;
            }
            _context.Campaigns.Add(campaign);
            _context.SaveChanges();
        }
    }
}
