using System;

namespace EmploymentPortal.DTOs
{
    public record CreateJobRequestDto(string Title, string Description, string Requirements, string SalaryRange, string Location);
    public record UpdateJobRequestDto(string? Title, string? Description, string? Requirements, string? SalaryRange, string? Location, string? Status);
    public record JobResponseDto(Guid Id, string Title, string Description, string Requirements, string SalaryRange, string Location, string Status, DateTime CreatedAt);
}