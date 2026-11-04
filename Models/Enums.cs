namespace EmploymentPortal.Models
{
    public enum UserRole
    {
        JobSeeker,
        Employer,
        Admin
    }

    public enum ApplicationStatus
    {
        APPLIED,
        SHORTLISTED,
        REJECTED,
        HIRED
    }

    public enum InterviewStatus
    {
        SCHEDULED,
        RESCHEDULED,
        COMPLETED,
        CANCELLED
    }
}