using System;
using System.Collections.Generic;

namespace EmploymentPortal.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.JobSeeker;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Profile? Profile { get; set; }
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}