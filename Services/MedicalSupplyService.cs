// ...existing code...
using BusinessObjects;
using Repositories;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MedicalSupplyService : IMedicalSupplyService
    {
        public async Task UpdateMedicalSupplyAsync(MedicalSupply medicalSupply)
        {
            await Task.Run(() => _medicalSupplyRepository.UpdateMedicalSuplly(medicalSupply));
        }

        private readonly IMedicalSupplyRepository _medicalSupplyRepository;

        public MedicalSupplyService()
        {
            _medicalSupplyRepository = new MedicalSupplyRepository();
        }

        public MedicalSupplyService(IMedicalSupplyRepository medicalSupplyRepository)
        {
            _medicalSupplyRepository = medicalSupplyRepository;
        }

        //1. Get all medical supplies
        public List<MedicalSupply> GetMedicalSupplies()
        {
            return _medicalSupplyRepository.GetMedicalSupplies();
        }

        //1a. Get all medical supplies async
        public async Task<List<MedicalSupply>> GetAllMedicalSuppliesAsync()
        {
            return await Task.FromResult(_medicalSupplyRepository.GetMedicalSupplies());
        }

        //2. Create a new medical supply
        public void CreateMedicalSupply(MedicalSupply medicalSupply)
        {
            _medicalSupplyRepository.CreateMedicalSupply(medicalSupply);
        }

        //3. Update an existing medical supply
        public void UpdateMedicalSuplly(MedicalSupply medicalSupply)
        {
            _medicalSupplyRepository.UpdateMedicalSuplly(medicalSupply);
        }

        //4. Delete a medical supply
        public void DeleteMedicalSupply(MedicalSupply medicalSupply)
        {
            _medicalSupplyRepository.DeleteMedicalSupply(medicalSupply);
        }

        //4a. Delete a medical supply async
        public Task DeleteMedicalSupplyAsync(Guid supplyId)
        {
            var supply = _medicalSupplyRepository.GetMedicalSupplies().FirstOrDefault(s => s.Id == supplyId);
            if (supply != null)
            {
                _medicalSupplyRepository.DeleteMedicalSupply(supply);
            }
            return Task.CompletedTask;
        }

        //5. Get a medical supply by ID
        public MedicalSupply? GetMedicalSupplyById(Guid id)
        {
            return _medicalSupplyRepository.GetMedicalSupplies().FirstOrDefault(s => s.Id == id);
        }

        //6. Search medical supplies
        public Task<List<MedicalSupply>> SearchMedicalSuppliesAsync(string searchTerm)
        {
            var allSupplies = _medicalSupplyRepository.GetMedicalSupplies();
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Task.FromResult(allSupplies);

            var filteredSupplies = allSupplies.Where(s =>
                s.SupplyName?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true ||
                s.Supplier?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) == true)
                .ToList();

            return Task.FromResult(filteredSupplies);
        }
    }
}
