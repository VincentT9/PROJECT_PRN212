using BusinessObjects;
using Repositories;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CampaignService : ICampaignService
    {
        CampaignRepository _campaignRepository = new CampaignRepository();
        public void CreateCampaign(Campaign campaign) => _campaignRepository.CreateCampaign(campaign);


        public void DeleteCampaign(Campaign campaign) => _campaignRepository.DeleteCampaign(campaign);


        public List<Campaign> GetCampaigns() => _campaignRepository.GetCampaigns();


        public void UpdateCampaign(Campaign campaign) => _campaignRepository.UpdateCampaign(campaign);

    }
}
