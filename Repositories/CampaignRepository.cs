using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;

namespace Repositories
{
    public class CampaignRepository : ICampaignRepository
    {
        CampaignDAO dao = new CampaignDAO();
        public void CreateCampaign(Campaign campaign)
        {
            dao.CreateCampaign(campaign);
        }

        public List<Campaign> GetAllCampaigns()
        {
            return dao.GetAllCampaigns();
        }
    }
}
