<<<<<<< HEAD
﻿using BusinessObjects;
using System;
=======
﻿using System;
>>>>>>> origin/quocbao
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
=======
using BusinessObjects;
>>>>>>> origin/quocbao

namespace Repositories.Interface
{
    public interface ICampaignRepository
    {
<<<<<<< HEAD
        public List<Campaign> GetCampaigns();
        public void CreateCampaign(Campaign campaign);
        public void UpdateCampaign(Campaign campaign);
        public void DeleteCampaign(Campaign campaign);
=======
        public List<Campaign> GetAllCampaigns();
        public void CreateCampaign(Campaign campaign);
>>>>>>> origin/quocbao
    }
}
