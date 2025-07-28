using BusinessObjects;
using Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

using Services.Interface;

namespace Services
{
    public class MedicalSupplyUsageService : IMedicalSupplyUsageService
    {
        private readonly MedicalSupplyUsageRepository _repository;
        public MedicalSupplyUsageService(MedicalSupplyUsageRepository repository)
        {
            _repository = repository;
        }
        public async Task AddMedicalSupplyUsagesAsync(List<MedicalSupplyUsage> usages)
        {
            await _repository.AddMedicalSupplyUsagesAsync(usages);
        }

        public List<MedicalSupplyUsage> GetUsagesByIncidentId(Guid incidentId)
        {
            return _repository.GetUsagesByIncidentId(incidentId);
        }
    }
}
