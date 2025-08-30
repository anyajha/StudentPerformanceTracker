using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPerformanceAPI.Data;
using StudentPerformanceAPI.Models;
using StudentPerformanceAPI.Models.DTOs;
using System.Linq;
using System.Threading.Tasks;
namespace StudentPerformanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AuthController(AppDbContext context) => _context = context;
        // POST: /api/auth/signup
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.Username) ||
                string.IsNullOrWhiteSpace(req.Password) || string.IsNullOrWhiteSpace(req.Role))
                return BadRequest("Username, Password and Role are required.");
            var exists = await _context.Users.AnyAsync(u => u.Username.ToLower() == req.Username.ToLower());
            if (exists) return Conflict("Username already exists.");
            var user = new User
            {
                Username = req.Username.Trim(),
                Password = req.Password,   // NOTE: plain text for demo. Hash in real apps.
                Role = req.Role.Trim()
            };
            // attach role entity
            switch (req.Role.Trim().ToLower())
            {
                case "student":
                    user.Student = new Student { Name = req.Name };
                    break;
                case "teacher":
                    user.Teacher = new Teacher { Name = req.Name };
                    break;
                case "admin":
                    user.Admin = new Admin { Name = req.Name };
                    break;
                default:
                    return BadRequest("Role must be Student, Teacher or Admin.");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            // auto-login style response
            var resp = await BuildLoginResponse(user.Id);
            return Ok(resp);
        }
        // POST: /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Models.DTOs.LoginRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Username and Password are required.");
            var user = await _context.Users
                .Include(u => u.Student)
                .Include(u => u.Teacher)
                .Include(u => u.Admin)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == req.Username.ToLower()
 && u.Password == req.Password);
            if (user == null) return Unauthorized("Invalid credentials.");
            var resp = await BuildLoginResponse(user.Id);
            return Ok(resp);
        }
        private async Task<LoginResponse> BuildLoginResponse(int userId)
        {
            var u = await _context.Users
                .Include(x => x.Student)
                .Include(x => x.Teacher)
                .Include(x => x.Admin)
                .FirstAsync(x => x.Id == userId);
            return new LoginResponse
            {
                UserId = u.Id,
                Username = u.Username,
                Role = u.Role,
                StudentId = u.Student?.Id,
                TeacherId = u.Teacher?.Id,
                AdminId = u.Admin?.Id,
                Name = u.Student?.Name ?? u.Teacher?.Name ?? u.Admin?.Name
            };
        }
    }
}