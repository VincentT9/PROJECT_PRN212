using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CampaignRepository : ICampaignRepository
    {
        CampaignDAO _campaignDAO = new CampaignDAO();
        public void CreateCampaign(Campaign campaign) => _campaignDAO.CreateCampaign(campaign);


        public void DeleteCampaign(Campaign campaign) => _campaignDAO.DeleteCampaign(campaign);


        public List<Campaign> GetCampaigns() => _campaignDAO.GetCampaigns();


        public void UpdateCampaign(Campaign campaign) => _campaignDAO.UpdateCampaign(campaign);

    }
}
