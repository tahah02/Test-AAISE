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
    [Route("api/jobs")]
    public class JobsController : ControllerBase
    {
        private readonly PortalDbContext _db;

        public JobsController(PortalDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetJobs([FromQuery] string? status)
        {
            var query = _db.Jobs.AsQueryable();
            if (!string.IsNullOrEmpty(status))
                query = query.Where(j => j.Status.ToLower() == status.ToLower());

            var jobs = await query.ToListAsync();
            return Ok(jobs.Select(j => new JobResponseDto(j.Id, j.Title, j.Description, j.Requirements, j.SalaryRange, j.Location, j.Status, j.CreatedAt)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJob(Guid id)
        {
            var j = await _db.Jobs.FindAsync(id);
            if (j == null) return NotFound(new { error = "Job not found" });
            return Ok(new JobResponseDto(j.Id, j.Title, j.Description, j.Requirements, j.SalaryRange, j.Location, j.Status, j.CreatedAt));
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobRequestDto dto)
        {
            var job = new Job
            {
                Title = dto.Title,
                Description = dto.Description,
                Requirements = dto.Requirements,
                SalaryRange = dto.SalaryRange,
                Location = dto.Location,
                Status = "ACTIVE",
            };
            _db.Jobs.Add(job);
            await _db.SaveChangesAsync();
            return StatusCode(201, new JobResponseDto(job.Id, job.Title, job.Description, job.Requirements, job.SalaryRange, job.Location, job.Status, job.CreatedAt));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(Guid id, [FromBody] UpdateJobRequestDto dto)
        {
            var job = await _db.Jobs.FindAsync(id);
            if (job == null) return NotFound(new { error = "Job not found" });

            if (dto.Title != null) job.Title = dto.Title;
            if (dto.Description != null) job.Description = dto.Description;
            if (dto.Requirements != null) job.Requirements = dto.Requirements;
            if (dto.SalaryRange != null) job.SalaryRange = dto.SalaryRange;
            if (dto.Location != null) job.Location = dto.Location;
            if (dto.Status != null) job.Status = dto.Status;

            await _db.SaveChangesAsync();
            return Ok(new JobResponseDto(job.Id, job.Title, job.Description, job.Requirements, job.SalaryRange, job.Location, job.Status, job.CreatedAt));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            var job = await _db.Jobs.FindAsync(id);
            if (job == null) return NotFound(new { error = "Job not found" });
            _db.Jobs.Remove(job);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}