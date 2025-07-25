using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services.Interface
{
    public interface ICampaignService
    {
        public List<Campaign> GetAllCampaigns();
        public void CreateCampaign(Campaign campaign);
    }
}
