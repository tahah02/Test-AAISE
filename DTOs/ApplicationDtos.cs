using System;
using EmploymentPortal.Models;

namespace EmploymentPortal.DTOs
{
    public record SubmitApplicationRequestDto(Guid JobId, Guid CandidateId, string? Notes);
    public record UpdateApplicationStatusDto(ApplicationStatus Status);
    public record ApplicationResponseDto(Guid Id, Guid JobId, Guid CandidateId, ApplicationStatus Status, string? Notes, DateTime AppliedAt, string? JobTitle, string? CandidateName);
}