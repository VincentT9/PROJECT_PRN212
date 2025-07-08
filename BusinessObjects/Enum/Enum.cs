using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalManagementSystem.Enum
{
    public enum UserRole
    {
        Admin,
        Parent,
        MedicalStaff
    }
    public enum Gender
    {
        Male, 
        Female, 
        Other
    }
    public enum RequestStatus
    {
        Pending,
        Received,
        Administered, 
        Returned
    }
    public enum IncidentType
    {
        Accident, 
        Fever, 
        Fall, 
        Epidemic, 
        Other
    }
    public enum IncidentStatus
    {
        Reported, 
        Processing, 
        Resolved
    }
    public enum SupplyType
    {
        Medicine, 
        MedicalEquipment, 
        FirstAid,
        Other
    }
    public enum CampaignStatus
    {
        Planned, 
        InProgress, 
        Completed, 
        Cancelled
    }
    public enum CampaignType
    {
        Vaccination, 
        HealthCheckup, 
    }
    public enum ConsultantStatus
    {
        Scheduled, 
        Completed, 
        Cancelled
    }
    public enum BlogStatus
    {
        Draft,
        Published
    }

    public enum  MedicationStatus
    {
        Taken,
        NotTaken,
        Missed
    }
}
