using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories.Interface
{
    public interface ICampaignRepository
    {
        public List<Campaign> GetCampaigns();
        public List<Campaign> GetAllCampaigns();
        public void CreateCampaign(Campaign campaign);
        public void UpdateCampaign(Campaign campaign);
        public void DeleteCampaign(Campaign campaign);
    }
}
