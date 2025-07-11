using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Interface;
using Services.Interface;

namespace Services
{
    public class ScheduleDetailService : IScheduleDetailService
    {
        private readonly IScheduleDetailRepository _scheduleDetailRepository;
        private readonly IConsentFormRepository _consentFormRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleDetailService()
        {
            _scheduleDetailRepository = new ScheduleDetailRepository();
            _consentFormRepository = new ConsentFormRepository();
            _scheduleRepository = new ScheduleRepository();
        }
        public bool AddScheduleDetail(Guid studentId, Guid scheduleId)
        {
            var schedule = _scheduleRepository.GetScheduleByScheduleId(scheduleId);
            if (schedule == null)
                return false;

            var campaignId = schedule.CampaignId;
            if (!_consentFormRepository.IsStudentApprovedByParent(studentId, campaignId))
                return false;
            var detail = new ScheduleDetail
            {
                Id = Guid.NewGuid(),
                StudentId = studentId,
                ScheduleId = scheduleId,
                VaccinationDate = DateTime.Now,         // hoặc để null nếu cần nhập sau
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now
            };

            _scheduleDetailRepository.AddScheduleDetail(detail);
            return true;
        }

        public bool ExistsInSchedule(Guid studentId, Guid scheduleId)
        {
            return _scheduleDetailRepository.ExistsInSchedule(studentId, scheduleId);
        }

        public List<Student> GetStudentsByScheduleId(Guid scheduleId)
        {
            return _scheduleDetailRepository.GetStudentsByScheduleId(scheduleId);
        }
        public bool IsStudentInSchedule(Guid studentId, Guid scheduleId)
        {
            return _scheduleDetailRepository.IsStudentInSchedule(studentId, scheduleId);
        }
        public List<ScheduleDetail> GetByScheduleId(Guid scheduleId)
        {
            return _scheduleDetailRepository.GetByScheduleId(scheduleId);
        }
        public ScheduleDetail? GetByStudentAndSchedule(Guid studentId, Guid scheduleId)
        {
            return _scheduleDetailRepository.GetByStudentAndSchedule(studentId, scheduleId);
        }
    }
}
