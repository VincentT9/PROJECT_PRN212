using BusinessObjects;
using Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IMedicalSupplyUsageService
    {
        Task AddMedicalSupplyUsagesAsync(List<MedicalSupplyUsage> usages);
    }
}
