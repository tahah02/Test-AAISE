using System;
using System.Collections.Generic;

namespace EmploymentPortal.Models
{
    public class Job
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? EmployerId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public string SalaryRange { get; set; } = string.Empty;
        public string Location { get; set; } = "Remote";
        public string Status { get; set; } = "ACTIVE";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User? Employer { get; set; }
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}