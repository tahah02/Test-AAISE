using System;

namespace EmploymentPortal.DTOs
{
    public record CreateCandidateRequestDto(string FullName, string Email, string PhoneNumber, string Qualification, int ExperienceYears);
    public record UpdateCandidateRequestDto(string? FullName, string? Email, string? PhoneNumber, string? Qualification, int? ExperienceYears, string? VerificationStatus);
    public record CandidateResponseDto(Guid Id, string FullName, string Email, string PhoneNumber, string Qualification, int ExperienceYears, string VerificationStatus, DateTime CreatedAt);
}