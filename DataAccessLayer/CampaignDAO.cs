using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CampaignDAO
    {
        SwpSchoolMedicalManagementSystemContext _context = new SwpSchoolMedicalManagementSystemContext();
        //1. Get all campaigns
        public List<Campaign> GetCampaigns()
        {
            return _context.Campaigns.ToList();
        }
        //2. Create a new campaign
        public void CreateCampaign(Campaign campaign)
        {
            try
            {
                _context.Campaigns.Add(campaign);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //3. Update an existing campaign
        public void UpdateCampaign(Campaign campaign)
        {
            try
            {
                _context.Entry<Campaign>(campaign).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //4. Delete a campaign
        public void DeleteCampaign(Campaign campaign)
        {
            try
            {
                var existCampaign = _context.Campaigns.SingleOrDefault(c => c.Id == campaign.Id);
                if (existCampaign != null)
                {
                    _context.Campaigns.Remove(existCampaign);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
