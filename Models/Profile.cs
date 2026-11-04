using System;

namespace EmploymentPortal.Models
{
    public class Profile
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string ResumeUrl { get; set; } = string.Empty;

        public User? User { get; set; }
    }
}