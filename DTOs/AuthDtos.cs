using System;
using EmploymentPortal.Models;

namespace EmploymentPortal.DTOs
{
    public record RegisterRequestDto(string Email, string Password, UserRole Role = UserRole.JobSeeker);
    public record RegisterResponseDto(Guid Id, string Email, UserRole Role);
    public record LoginRequestDto(string Email, string Password);
    public record LoginResponseDto(string Token, string Email, UserRole Role);
}