using BusinessObjects;
using DataAccessLayer;
using Repositories.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class MedicalSupplyUsageRepository : Interface.IMedicalSupplyUsageRepository
    {
        private readonly MedicalSupplyUsageDAO _dao;
        public MedicalSupplyUsageRepository()
        {
            _dao = new MedicalSupplyUsageDAO();
        }

        public async Task AddMedicalSupplyUsagesAsync(List<MedicalSupplyUsage> usages)
        {
           await _dao.AddMedicalSupplyUsagesAsync(usages);
        }

        public List<MedicalSupplyUsage> GetUsagesByIncidentId(Guid incidentId)
        {
            return _dao.GetUsagesByIncidentId(incidentId);
        }
    }
}
