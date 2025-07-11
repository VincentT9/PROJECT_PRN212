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
    public class CampaignService : ICampaignService
    {
        ICampaignRepository compaignRepository;
        public CampaignService()
        {
            compaignRepository = new CampaignRepository();
        }
        public void CreateCampaign(Campaign campaign)
        {
            compaignRepository.CreateCampaign(campaign);
        }

        public List<Campaign> GetAllCampaigns()
        {
            return compaignRepository.GetAllCampaigns();
        }
    }
}
