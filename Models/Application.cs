using System;
using System.Collections.Generic;

namespace EmploymentPortal.Models
{
    public class Application
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid JobId { get; set; }
        public Guid CandidateId { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.APPLIED;
        public string? Notes { get; set; }
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        public Job? Job { get; set; }
        public Candidate? Candidate { get; set; }
        public ICollection<Interview> Interviews { get; set; } = new List<Interview>();
    }
}