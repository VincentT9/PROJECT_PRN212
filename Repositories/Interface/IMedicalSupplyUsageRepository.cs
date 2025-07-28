using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IMedicalSupplyUsageRepository
    {
        public List<MedicalSupplyUsage> GetUsagesByIncidentId(Guid incidentId);
        public Task AddMedicalSupplyUsagesAsync(List<MedicalSupplyUsage> usages);
    }
}
