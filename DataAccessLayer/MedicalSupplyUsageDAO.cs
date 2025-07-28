using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class MedicalSupplyUsageDAO
    {
        private readonly SwpSchoolMedicalManagementSystemContext _context;

        public MedicalSupplyUsageDAO()
        {
            _context = new SwpSchoolMedicalManagementSystemContext();
        }

        // Lấy danh sách vật tư sử dụng theo IncidentId
        public List<MedicalSupplyUsage> GetUsagesByIncidentId(Guid incidentId )
        {
            return _context.MedicalSupplyUsages
                .Where(u => u.IncidentId == incidentId)
                .Select(u => new MedicalSupplyUsage
                {
                    IncidentId = u.IncidentId,
                    SupplyId = u.SupplyId,
                    QuantityUsed = u.QuantityUsed,
                    UsageDate = u.UsageDate,
                    Notes = u.Notes,
                    Supply = u.Supply // đảm bảo navigation property được load
                })
                .ToList();
        }

        public async Task AddMedicalSupplyUsagesAsync(List<MedicalSupplyUsage> usages)
        {
            try
            {
                if (usages == null || usages.Count == 0) return;
                // Gộp các usage trùng IncidentId + SupplyId
                var mergedUsages = usages
                    .GroupBy(u => new { u.IncidentId, u.SupplyId })
                    .Select(g =>
                    {
                        var first = g.First();
                        first.QuantityUsed = g.Sum(x => x.QuantityUsed);
                        return first;
                    })
                    .ToList();

                foreach (var usage in mergedUsages)
                {
                    var exist = _context.MedicalSupplyUsages.FirstOrDefault(u => u.IncidentId == usage.IncidentId && u.SupplyId == usage.SupplyId);
                    if (exist != null)
                    {
                        exist.QuantityUsed = usage.QuantityUsed;
                        exist.UsageDate = usage.UsageDate;
                        exist.Notes = usage.Notes;
                        // _context.MedicalSupplyUsages.Update(exist); // không cần thiết với EF
                    }
                    else
                    {
                        await _context.MedicalSupplyUsages.AddAsync(usage);
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("An error occurred while adding medical supply usages.", ex);
            }
        }
    }
}
