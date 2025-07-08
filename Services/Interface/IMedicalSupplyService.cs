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
        public MedicalSupply GetMedicalSupplyById(int id);
    }
}
