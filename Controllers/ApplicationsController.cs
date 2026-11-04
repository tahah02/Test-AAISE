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
    [Route("api/applications")]
    public class ApplicationsController : ControllerBase
    {
        private readonly PortalDbContext _db;

        public ApplicationsController(PortalDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetApplications([FromQuery] Guid? jobId, [FromQuery] Guid? candidateId)
        {
            var query = _db.Applications.Include(a => a.Job).Include(a => a.Candidate).AsQueryable();
            if (jobId.HasValue) query = query.Where(a => a.JobId == jobId.Value);
            if (candidateId.HasValue) query = query.Where(a => a.CandidateId == candidateId.Value);

            var list = await query.ToListAsync();
            return Ok(list.Select(a => new ApplicationResponseDto(
                a.Id,
                a.JobId,
                a.CandidateId,
                a.Status,
                a.Notes,
                a.AppliedAt,
                a.Job?.Title,
                a.Candidate?.FullName
            )));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplication(Guid id)
        {
            var a = await _db.Applications.Include(a => a.Job).Include(a => a.Candidate).FirstOrDefaultAsync(x => x.Id == id);
            if (a == null) return NotFound(new { error = "Application not found" });
            return Ok(new ApplicationResponseDto(
                a.Id,
                a.JobId,
                a.CandidateId,
                a.Status,
                a.Notes,
                a.AppliedAt,
                a.Job?.Title,
                a.Candidate?.FullName
            ));
        }

        [HttpPost]
        public async Task<IActionResult> SubmitApplication([FromBody] SubmitApplicationRequestDto dto)
        {
            var job = await _db.Jobs.FindAsync(dto.JobId);
            if (job == null) return BadRequest(new { error = "Referenced Job not found" });

            var candidate = await _db.Candidates.FindAsync(dto.CandidateId);
            if (candidate == null) return BadRequest(new { error = "Referenced Candidate not found" });

            var existing = await _db.Applications.AnyAsync(a => a.JobId == dto.JobId && a.CandidateId == dto.CandidateId);
            if (existing) return Conflict(new { error = "Duplicate application: Candidate has already applied for this job" });

            var app = new Application
            {
                JobId = dto.JobId,
                CandidateId = dto.CandidateId,
                Notes = dto.Notes,
                Status = ApplicationStatus.APPLIED,
            };

            _db.Applications.Add(app);
            await _db.SaveChangesAsync();

            return StatusCode(201, new ApplicationResponseDto(
                app.Id,
                app.JobId,
                app.CandidateId,
                app.Status,
                app.Notes,
                app.AppliedAt,
                job.Title,
                candidate.FullName
            ));
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateApplicationStatusDto dto)
        {
            var app = await _db.Applications.Include(a => a.Job).Include(a => a.Candidate).FirstOrDefaultAsync(x => x.Id == id);
            if (app == null) return NotFound(new { error = "Application not found" });

            app.Status = dto.Status;
            await _db.SaveChangesAsync();

            return Ok(new ApplicationResponseDto(
                app.Id,
                app.JobId,
                app.CandidateId,
                app.Status,
                app.Notes,
                app.AppliedAt,
                app.Job?.Title,
                app.Candidate?.FullName
            ));
        }
    }
}