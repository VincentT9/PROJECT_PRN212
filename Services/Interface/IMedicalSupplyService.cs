using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IMedicalSupplyService
    {
        public List<MedicalSupply> GetMedicalSupplies();
        public void CreateMedicalSupply(MedicalSupply medicalSupply);
        public void UpdateMedicalSuplly(MedicalSupply medicalSupply);
        public void DeleteMedicalSupply(MedicalSupply medicalSupply);
        public MedicalSupply? GetMedicalSupplyById(Guid id);
        public Task<List<MedicalSupply>> GetAllMedicalSuppliesAsync();
        public Task DeleteMedicalSupplyAsync(Guid supplyId);
        public Task<List<MedicalSupply>> SearchMedicalSuppliesAsync(string searchTerm);
    }
}
