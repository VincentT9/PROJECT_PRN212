

using BusinessObjects;

namespace Repositories.Interface
{
    public interface IMedicalSupplyRepository
    {
        public List<MedicalSupply> GetMedicalSupplies();
        public void CreateMedicalSupply(MedicalSupply medicalSupply);
        public void UpdateMedicalSuplly(MedicalSupply medicalSupply);
        public void DeleteMedicalSupply(MedicalSupply medicalSupply);
        public MedicalSupply GetMedicalSupplyById(int id);
    }
}
