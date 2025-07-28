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

                    existingSchedule.ScheduledDate = DateTime.SpecifyKind(schedule.ScheduledDate, DateTimeKind.Utc);

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
                    .Include(s => s.Campaign)
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

                var utcNow = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

                return _context.Schedules
                    .Include(s => s.Campaign)
                    .Where(s => s.ScheduledDate >= DateTimeOffset.UtcNow)
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
                    .Include(sd => sd.VaccinationResult)
                    .Include(sd => sd.HealthCheckupResult)
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

        
        public void UpdateStudentVaccinationStatus(Guid studentId, Guid scheduleId, string status, string? updatedBy = null)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var scheduleDetail = _context.ScheduleDetails
                    .FirstOrDefault(sd => sd.StudentId == studentId && sd.ScheduleId == scheduleId);
                if (scheduleDetail != null)
                {
                    
                    var vaccinationResult = _context.VaccinationResults
                        .FirstOrDefault(vr => vr.ScheduleDetailId == scheduleDetail.Id);
                        
                    if (vaccinationResult == null)
                    {
                        
                        vaccinationResult = new VaccinationResult
                        {
                            Id = Guid.NewGuid(),
                            ScheduleDetailId = scheduleDetail.Id,
                            Notes = $"Status: {status}",
                            CreatedBy = updatedBy,
                            UpdatedBy = updatedBy,
                            CreateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                            UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                        };
                        _context.VaccinationResults.Add(vaccinationResult);
                    }
                    else
                    {
                        
                        vaccinationResult.Notes = $"Status: {status}";
                        vaccinationResult.UpdatedBy = updatedBy;
                        vaccinationResult.UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }
                    
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
        
        public void UpdateVaccinationResult(Guid studentId, Guid scheduleId, string dosage, string sideEffects, string notes, string? updatedBy = null)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                
                var scheduleDetail = _context.ScheduleDetails
                    .FirstOrDefault(sd => sd.StudentId == studentId && sd.ScheduleId == scheduleId);
                
                if (scheduleDetail == null)
                {
                    scheduleDetail = new ScheduleDetail
                    {
                        Id = Guid.NewGuid(),
                        StudentId = studentId,
                        ScheduleId = scheduleId,
                        VaccinationDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        CreateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                    };
                    _context.ScheduleDetails.Add(scheduleDetail);
                    _context.SaveChanges();
                }
                
                
                var vaccinationResult = _context.VaccinationResults
                    .FirstOrDefault(vr => vr.ScheduleDetailId == scheduleDetail.Id);
                
                if (vaccinationResult == null)
                {
                   
                    vaccinationResult = new VaccinationResult
                    {
                        Id = Guid.NewGuid(),
                        ScheduleDetailId = scheduleDetail.Id,
                        DosageGiven = dosage,
                        SideEffects = sideEffects,
                        Notes = notes,
                        CreatedBy = updatedBy,
                        UpdatedBy = updatedBy,
                        CreateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                    };
                    _context.VaccinationResults.Add(vaccinationResult);
                }
                else
                {
                    vaccinationResult.DosageGiven = dosage;
                    vaccinationResult.SideEffects = sideEffects;
                    vaccinationResult.Notes = notes;
                    vaccinationResult.UpdatedBy = updatedBy;
                    vaccinationResult.UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                }
                
                scheduleDetail.UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating vaccination result: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public void AddStudentToSchedule(ScheduleDetail scheduleDetail)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var exists = _context.ScheduleDetails
                    .Any(sd => sd.StudentId == scheduleDetail.StudentId && sd.ScheduleId == scheduleDetail.ScheduleId);
                
                if (exists)
                {
                    return;
                }
                
                if (scheduleDetail.VaccinationDate != default)
                {
                    scheduleDetail.VaccinationDate = DateTime.SpecifyKind(scheduleDetail.VaccinationDate, DateTimeKind.Utc);
                }
                
                scheduleDetail.CreateAt = DateTime.SpecifyKind(scheduleDetail.CreateAt, DateTimeKind.Utc);
                scheduleDetail.UpdateAt = DateTime.SpecifyKind(scheduleDetail.UpdateAt, DateTimeKind.Utc);
                
                _context.ScheduleDetails.Add(scheduleDetail);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding student to schedule: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }

        public void UpdateStudentHealthCheckupStatus(Guid studentId, Guid scheduleId, string notes, HealthCheckupResult healthCheckupResult)
        {
            try
            {
                using var _context = new SwpSchoolMedicalManagementSystemContext();
                var scheduleDetail = _context.ScheduleDetails
                    .FirstOrDefault(sd => sd.StudentId == studentId && sd.ScheduleId == scheduleId);
                
                if (scheduleDetail == null)
                {
                    scheduleDetail = new ScheduleDetail
                    {
                        Id = Guid.NewGuid(),
                        StudentId = studentId,
                        ScheduleId = scheduleId,
                        VaccinationDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        CreateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                    };
                    _context.ScheduleDetails.Add(scheduleDetail);
                    _context.SaveChanges();
                }
                
                healthCheckupResult.ScheduleDetailId = scheduleDetail.Id;
                
                healthCheckupResult.CreateAt = DateTime.SpecifyKind(healthCheckupResult.CreateAt, DateTimeKind.Utc);
                healthCheckupResult.UpdateAt = DateTime.SpecifyKind(healthCheckupResult.UpdateAt, DateTimeKind.Utc);
                
                var existingResult = _context.HealthCheckupResults
                    .FirstOrDefault(hcr => hcr.ScheduleDetailId == scheduleDetail.Id);
                
                if (existingResult != null)
                {
                    existingResult.Height = healthCheckupResult.Height;
                    existingResult.Weight = healthCheckupResult.Weight;
                    existingResult.VisionLeftResult = healthCheckupResult.VisionLeftResult;
                    existingResult.VisionRightResult = healthCheckupResult.VisionRightResult;
                    existingResult.HearingLeftResult = healthCheckupResult.HearingLeftResult;
                    existingResult.HearingRightResult = healthCheckupResult.HearingRightResult;
                    existingResult.BloodPressureSys = healthCheckupResult.BloodPressureSys;
                    existingResult.BloodPressureDia = healthCheckupResult.BloodPressureDia;
                    existingResult.HeartRate = healthCheckupResult.HeartRate;
                    existingResult.DentalCheckupResult = healthCheckupResult.DentalCheckupResult;
                    existingResult.OtherResults = healthCheckupResult.OtherResults;
                    existingResult.UpdateAt = healthCheckupResult.UpdateAt;
                }
                else
                {
                    _context.HealthCheckupResults.Add(healthCheckupResult);
                }
                scheduleDetail.UpdateAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating student health checkup status: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                throw;
            }
        }
    }
}
