using System;

namespace EmploymentPortal.Models
{
    public class Interview
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ApplicationId { get; set; }
        public DateTime ScheduledAt { get; set; } = DateTime.UtcNow.AddDays(2);
        public string LocationOrLink { get; set; } = "https://meet.google.com/portal-interview";
        public string InterviewerName { get; set; } = "Lead Recruiter";
        public InterviewStatus Status { get; set; } = InterviewStatus.SCHEDULED;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Application? Application { get; set; }
    }
}