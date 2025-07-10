using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ICampaignService
    {
        public List<Campaign> GetCampaigns();
        public void CreateCampaign(Campaign campaign);
        public void UpdateCampaign(Campaign campaign);
        public void DeleteCampaign(Campaign campaign);
    }
}
