using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;          // Include, ToListAsync
using System.Linq;                            // Where, Select
using System.Threading.Tasks;
using StudentPerformanceAPI.Data;
using StudentPerformanceAPI.Models.DTOs;      // MarksViewDto, AttendanceViewDto
namespace StudentPerformanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StudentController(AppDbContext context) => _context = context;
        // GET: api/student/{studentId}/marks
        [HttpGet("{studentId}/marks")]
        public async Task<IActionResult> GetMarks(int studentId)
        {
            var studentExists = await _context.Students.AnyAsync(s => s.Id == studentId);
            if (!studentExists) return NotFound("Student not found");
            var marks = await _context.Assessments
                .AsNoTracking()
                .Include(a => a.Subject)
                .Where(a => a.StudentId == studentId)
                .Select(a => new MarksViewDto
                {
                    SubjectName = a.Subject != null ? a.Subject.SubjectName : "(Unknown)",
                    Term = a.Term,
                    Marks = a.Marks
                })
                .ToListAsync();
            return Ok(marks);
        }
        // GET: api/student/{studentId}/attendance
        [HttpGet("{studentId}/attendance")]
        public async Task<IActionResult> GetAttendance(int studentId)
        {
            var studentExists = await _context.Students.AnyAsync(s => s.Id == studentId);
            if (!studentExists) return NotFound("Student not found");
            var attendance = await _context.Attendances
                .AsNoTracking()
                .Where(a => a.StudentId == studentId)
                .Select(a => new AttendanceViewDto
                {
                    Date = a.Date,
                    IsPresent = a.IsPresent
                })
                .ToListAsync();
            return Ok(attendance);
        }
    }
}