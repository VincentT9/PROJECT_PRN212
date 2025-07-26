using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalManagementSystem.Enum;

namespace DataAccessLayer
{
    public class CampaignDAO
    {
        //1. Get all campaigns
        public List<Campaign> GetCampaigns()
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                return _context.Campaigns
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting campaigns: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }

        }

        //1.1. Get campaigns by status
        public List<Campaign> GetAllCampaigns()
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                return _context.Campaigns
                    .AsNoTracking()
                    .Where(c => c.Status == (int)CampaignStatus.InProgress)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting campaigns by status: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }


        //2. Create a new campaign
        public void CreateCampaign(Campaign campaign)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var existingCampaign = _context.Campaigns
                    .AsNoTracking()
                    .FirstOrDefault(c => c.Id == campaign.Id);
                
                if(existingCampaign != null)
                {
                    return;
                }
                
                // Ensure date properties have UTC kind
                campaign.CreateAt = DateTime.SpecifyKind(campaign.CreateAt, DateTimeKind.Utc);
                campaign.UpdateAt = DateTime.SpecifyKind(campaign.UpdateAt, DateTimeKind.Utc);
                
                _context.Campaigns.Add(campaign);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating campaign: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        //3. Update an existing campaign
        public void UpdateCampaign(Campaign campaign)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var existingCampaign = _context.Campaigns
                    .FirstOrDefault(c => c.Id == campaign.Id);
                
                if (existingCampaign == null)
                {
                    throw new Exception($"Campaign with ID {campaign.Id} not found");
                }
                
                // Update properties
                existingCampaign.Name = campaign.Name;
                existingCampaign.Description = campaign.Description;
                existingCampaign.Status = campaign.Status;
                existingCampaign.Type = campaign.Type;
                existingCampaign.UpdatedBy = campaign.UpdatedBy;
                existingCampaign.UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating campaign: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        //4. Delete a campaign
        public void DeleteCampaign(Campaign campaign)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var existingCampaign = _context.Campaigns
                    .FirstOrDefault(c => c.Id == campaign.Id);
                
                if (existingCampaign != null)
                {
                    _context.Campaigns.Remove(existingCampaign);
                    _context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting campaign: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }
    }
}
