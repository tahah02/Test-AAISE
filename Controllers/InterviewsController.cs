using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmploymentPortal.Data;
using EmploymentPortal.DTOs;
using EmploymentPortal.Models;

namespace EmploymentPortal.Controllers
{
    [ApiController]
    [Route("api/interviews")]
    public class InterviewsController : ControllerBase
    {
        private readonly PortalDbContext _db;

        public InterviewsController(PortalDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetInterviews([FromQuery] Guid? applicationId)
        {
            var query = _db.Interviews.AsQueryable();
            if (applicationId.HasValue) query = query.Where(i => i.ApplicationId == applicationId.Value);

            var list = await query.ToListAsync();
            return Ok(list.Select(i => new InterviewResponseDto(
                i.Id,
                i.ApplicationId,
                i.ScheduledAt,
                i.LocationOrLink,
                i.InterviewerName,
                i.Status,
                i.Notes,
                i.CreatedAt
            )));
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleInterview([FromBody] ScheduleInterviewRequestDto dto)
        {
            var app = await _db.Applications.FindAsync(dto.ApplicationId);
            if (app == null) return BadRequest(new { error = "Referenced Application not found" });

            var interview = new Interview
            {
                ApplicationId = dto.ApplicationId,
                ScheduledAt = dto.ScheduledAt,
                LocationOrLink = dto.LocationOrLink,
                InterviewerName = dto.InterviewerName,
                Notes = dto.Notes,
                Status = InterviewStatus.SCHEDULED,
            };

            _db.Interviews.Add(interview);
            await _db.SaveChangesAsync();

            return StatusCode(201, new InterviewResponseDto(
                interview.Id,
                interview.ApplicationId,
                interview.ScheduledAt,
                interview.LocationOrLink,
                interview.InterviewerName,
                interview.Status,
                interview.Notes,
                interview.CreatedAt
            ));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> RescheduleInterview(Guid id, [FromBody] RescheduleInterviewRequestDto dto)
        {
            var interview = await _db.Interviews.FindAsync(id);
            if (interview == null) return NotFound(new { error = "Interview not found" });

            interview.ScheduledAt = dto.ScheduledAt;
            if (dto.LocationOrLink != null) interview.LocationOrLink = dto.LocationOrLink;
            if (dto.InterviewerName != null) interview.InterviewerName = dto.InterviewerName;
            if (dto.Status.HasValue) interview.Status = dto.Status.Value;

            await _db.SaveChangesAsync();

            return Ok(new InterviewResponseDto(
                interview.Id,
                interview.ApplicationId,
                interview.ScheduledAt,
                interview.LocationOrLink,
                interview.InterviewerName,
                interview.Status,
                interview.Notes,
                interview.CreatedAt
            ));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelInterview(Guid id)
        {
            var interview = await _db.Interviews.FindAsync(id);
            if (interview == null) return NotFound(new { error = "Interview not found" });

            interview.Status = InterviewStatus.CANCELLED;
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}