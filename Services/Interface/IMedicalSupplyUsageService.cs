using BusinessObjects;
using Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IMedicalSupplyUsageService
    {
        public List<MedicalSupplyUsage> GetUsagesByIncidentId(Guid incidentId);
        public Task AddMedicalSupplyUsagesAsync(List<MedicalSupplyUsage> usages);
    }
}
