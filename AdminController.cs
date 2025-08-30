using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPerformanceAPI.Data;
using System.Linq;
using System.Threading.Tasks;
namespace StudentPerformanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context) => _context = context;
        // 1️⃣ Student Report
        [HttpGet("students-report")]
        public async Task<IActionResult> GetStudentsReport()
        {
            var report = await _context.Students
                .AsNoTracking()
                .Select(s => new
                {
                    student = s.Name,
                    attendanceCount = s.Attendances.Count(),
                    marks = _context.Assessments
                        .AsNoTracking()
                        .Where(a => a.StudentId == s.Id)
                        .Join(_context.Subjects.AsNoTracking(),
                              a => a.SubjectId,
                              sub => sub.Id,
                              (a, sub) => new
                              {
                                  subject = sub.SubjectName,
                                  term = a.Term,
                                  marks = a.Marks
                              })
                        .OrderBy(m => m.subject)
                        .ThenBy(m => m.term)
                        .ToList()
                })
                .ToListAsync();
            return Ok(report);
        }
        // 2️⃣ Teacher Report
        [HttpGet("teachers-report")]
        public async Task<IActionResult> GetTeachersReport()
        {
            var report = await _context.Teachers
                .AsNoTracking()
                .Select(t => new
                {
                    teacher = t.Name,
                    subjects = t.Subjects.Select(s => s.SubjectName).ToList(),
                    subjectsCount = t.Subjects.Count(),
                    assessmentsCount = _context.Assessments.Count(a => a.TeacherId == t.Id)
                })
                .ToListAsync();
            return Ok(report);
        }
    }
}