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
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly PortalDbContext _db;
        private readonly ITokenService _tokenService;

        public AuthController(PortalDbContext db, ITokenService tokenService)
        {
            _db = db;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower()))
                return BadRequest(new { error = "User with this email already exists" });

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = BCryptOrPlain(dto.Password),
                Role = dto.Role,
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return StatusCode(201, new RegisterResponseDto(user.Id, user.Email, user.Role));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());
            if (user == null || user.PasswordHash != BCryptOrPlain(dto.Password))
                return Unauthorized(new { error = "Invalid credentials" });

            var token = _tokenService.GenerateToken(user);
            return Ok(new LoginResponseDto(token, user.Email, user.Role));
        }

        private static string BCryptOrPlain(string pass) => Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(pass));
    }
}