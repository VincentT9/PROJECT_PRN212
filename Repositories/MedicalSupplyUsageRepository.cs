using BusinessObjects;
using DataAccessLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class MedicalSupplyUsageRepository
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
    }
}
