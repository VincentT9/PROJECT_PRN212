using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class ScheduleDAO
    {
        public List<Schedule> GetSchedules()
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                return _context.Schedules
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting schedules: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public List<Schedule> GetAllSchedules()
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                return _context.Schedules
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all schedules: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public void CreateSchedule(Schedule schedule)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var existingSchedule = _context.Schedules.FirstOrDefault(s => s.Id == schedule.Id);
                if (existingSchedule != null)
                {
                    return;
                }

                // Convert DateTime values to UTC
                schedule.ScheduledDate = DateTime.SpecifyKind(schedule.ScheduledDate, DateTimeKind.Utc);

                if (schedule.CreateAt == default)
                {
                    schedule.CreateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                }
                else
                {
                    schedule.CreateAt = DateTime.SpecifyKind(schedule.CreateAt, DateTimeKind.Utc);
                }

                schedule.UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                _context.Schedules.Add(schedule);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating schedule: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public void UpdateSchedule(Schedule schedule)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var existingSchedule = _context.Schedules.FirstOrDefault(s => s.Id == schedule.Id);
                if (existingSchedule != null)
                {
                    // Update ScheduledDate with UTC kind
                    existingSchedule.ScheduledDate = DateTime.SpecifyKind(schedule.ScheduledDate, DateTimeKind.Utc);

                    // Update other properties
                    existingSchedule.CampaignId = schedule.CampaignId;
                    existingSchedule.Location = schedule.Location;
                    existingSchedule.Notes = schedule.Notes;
                    existingSchedule.UpdatedBy = schedule.UpdatedBy;
                    existingSchedule.UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating schedule: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public void DeleteSchedule(Schedule schedule)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var existingSchedule = _context.Schedules.FirstOrDefault(s => s.Id == schedule.Id);
                if (existingSchedule != null)
                {
                    _context.Schedules.Remove(existingSchedule);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting schedule: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public Schedule GetScheduleByScheduleId(Guid scheduleId)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                return _context.Schedules
                    .AsNoTracking()
                    .FirstOrDefault(s => s.Id == scheduleId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting schedule by ID: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public List<Schedule> GetActiveSchedules()
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();

                // Use UTC now for comparison
                var utcNow = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

                return _context.Schedules
                    .Include(s => s.Campaign)
                    .Where(s => s.ScheduledDate >= utcNow)
                    .OrderBy(s => s.ScheduledDate)
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting active schedules: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public List<ScheduleDetail> GetScheduleDetailsByScheduleId(Guid scheduleId)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                return _context.ScheduleDetails
                    .Where(sd => sd.ScheduleId == scheduleId)
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting schedule details: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public List<Guid> GetStudentIdsByScheduleId(Guid scheduleId)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                return _context.ScheduleDetails
                    .Where(sd => sd.ScheduleId == scheduleId)
                    .Select(sd => sd.StudentId)
                    .Where(id => id.HasValue)
                    .Select(id => id.Value)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting student IDs by schedule ID: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public void UpdateStudentVaccinationStatus(Guid studentId, Guid scheduleId, string status)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var scheduleDetail = _context.ScheduleDetails
                    .FirstOrDefault(sd => sd.StudentId == studentId && sd.ScheduleId == scheduleId);
                if (scheduleDetail != null)
                {
                    scheduleDetail.Status = status;
                    scheduleDetail.UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating student vaccination status: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }
    }
}
