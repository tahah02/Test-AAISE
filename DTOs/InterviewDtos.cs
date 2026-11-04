using System;
using EmploymentPortal.Models;

namespace EmploymentPortal.DTOs
{
    public record ScheduleInterviewRequestDto(Guid ApplicationId, DateTime ScheduledAt, string LocationOrLink, string InterviewerName, string? Notes);
    public record RescheduleInterviewRequestDto(DateTime ScheduledAt, string? LocationOrLink, string? InterviewerName, InterviewStatus? Status);
    public record InterviewResponseDto(Guid Id, Guid ApplicationId, DateTime ScheduledAt, string LocationOrLink, string InterviewerName, InterviewStatus Status, string? Notes, DateTime CreatedAt);
}