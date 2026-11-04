using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmploymentPortal.Data;
using EmploymentPortal.DTOs;
using EmploymentPortal.Models;
using EmploymentPortal.Services;

namespace EmploymentPortal.Controllers
{
    [ApiController]
    [Route("api/candidates")]
    public class CandidatesController : ControllerBase
    {
        private readonly PortalDbContext _db;
        private readonly IVendorService _vendorService;

        public CandidatesController(PortalDbContext db, IVendorService vendorService)
        {
            _db = db;
            _vendorService = vendorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCandidates()
        {
            var list = await _db.Candidates.ToListAsync();
            return Ok(list.Select(c => new CandidateResponseDto(c.Id, c.FullName, c.Email, c.PhoneNumber, c.Qualification, c.ExperienceYears, c.VerificationStatus, c.CreatedAt)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCandidate(Guid id)
        {
            var c = await _db.Candidates.FindAsync(id);
            if (c == null) return NotFound(new { error = "Candidate not found" });
            return Ok(new CandidateResponseDto(c.Id, c.FullName, c.Email, c.PhoneNumber, c.Qualification, c.ExperienceYears, c.VerificationStatus, c.CreatedAt));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCandidate([FromBody] CreateCandidateRequestDto dto)
        {
            if (await _db.Candidates.AnyAsync(c => c.Email.ToLower() == dto.Email.ToLower()))
                return BadRequest(new { error = "Candidate with this email already exists" });

            var isVerified = await _vendorService.VerifyCandidateIdentityAsync(dto.Email, dto.FullName);

            var c = new Candidate
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Qualification = dto.Qualification,
                ExperienceYears = dto.ExperienceYears,
                VerificationStatus = isVerified ? "VERIFIED" : "PENDING",
            };

            _db.Candidates.Add(c);
            await _db.SaveChangesAsync();

            return StatusCode(201, new CandidateResponseDto(c.Id, c.FullName, c.Email, c.PhoneNumber, c.Qualification, c.ExperienceYears, c.VerificationStatus, c.CreatedAt));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCandidate(Guid id, [FromBody] UpdateCandidateRequestDto dto)
        {
            var c = await _db.Candidates.FindAsync(id);
            if (c == null) return NotFound(new { error = "Candidate not found" });

            if (dto.FullName != null) c.FullName = dto.FullName;
            if (dto.Email != null) c.Email = dto.Email;
            if (dto.PhoneNumber != null) c.PhoneNumber = dto.PhoneNumber;
            if (dto.Qualification != null) c.Qualification = dto.Qualification;
            if (dto.ExperienceYears.HasValue) c.ExperienceYears = dto.ExperienceYears.Value;
            if (dto.VerificationStatus != null) c.VerificationStatus = dto.VerificationStatus;

            await _db.SaveChangesAsync();
            return Ok(new CandidateResponseDto(c.Id, c.FullName, c.Email, c.PhoneNumber, c.Qualification, c.ExperienceYears, c.VerificationStatus, c.CreatedAt));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidate(Guid id)
        {
            var c = await _db.Candidates.FindAsync(id);
            if (c == null) return NotFound(new { error = "Candidate not found" });
            _db.Candidates.Remove(c);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}