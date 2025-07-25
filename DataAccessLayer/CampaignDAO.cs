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
        //1. Get all campaigns
        public List<Campaign> GetCampaigns()
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.Campaigns.ToList();
        }

        //1.1. Get campaigns by status
        public List<Campaign> GetAllCampaigns()
        {
            using var context = new SwpSchoolMedicalManagementSystemContext();
            return context.Campaigns.
                Where(c => c.Status ==
                (int)CampaignStatus.InProgress).ToList();
        }

        //2. Create a new campaign
        public void CreateCampaign(Campaign campaign)
        {
            try
            {
                using var context = new SwpSchoolMedicalManagementSystemContext();
                var existingCampaign = context.Campaigns.FirstOrDefault(c => c.Id == campaign.Id);
                if (existingCampaign != null)
                {
                    return;
                }
                context.Campaigns.Add(campaign);
                context.SaveChanges();
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
                using var context = new SwpSchoolMedicalManagementSystemContext();
                context.Entry<Campaign>(campaign).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
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
                using var context = new SwpSchoolMedicalManagementSystemContext();
                var existCampaign = context.Campaigns.SingleOrDefault(c => c.Id == campaign.Id);
                if (existCampaign != null)
                {
                    context.Campaigns.Remove(existCampaign);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
