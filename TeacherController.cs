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
    public class TeacherController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TeacherController(AppDbContext context) => _context = context;
        // ---------- Helper: numeric marks -> letter ----------
        private static string GetGradeLetter(int marks)
        {
            if (marks >= 85) return "A";
            if (marks >= 70) return "B";
            return "C";
        }
        // ---------- 1) Add a subject by NAME ----------
        // POST: /api/teacher/{teacherId}/subject
        [HttpPost("{teacherId}/subject")]
        public async Task<IActionResult> AddSubject(int teacherId, [FromBody] SubjectCreateDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Subject name is required.");
            // ensure teacher exists
            var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == teacherId);
            if (!teacherExists) return NotFound("Teacher not found.");
            var name = dto.Name.Trim();
            // enforce uniqueness per teacher
            var exists = await _context.Subjects
                .AnyAsync(s => s.TeacherId == teacherId && s.SubjectName.ToLower() == name.ToLower());
            if (exists) return Conflict("Subject already exists for this teacher.");
            var subject = new Subject { SubjectName = name, TeacherId = teacherId };
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return Ok(new { subject.Id, subject.SubjectName, subject.TeacherId });
        }
        // ---------- 2) List this teacher's subjects ----------
        // GET: /api/teacher/{teacherId}/subjects
        [HttpGet("{teacherId}/subjects")]
        public async Task<IActionResult> GetSubjects(int teacherId)
        {
            var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == teacherId);
            if (!teacherExists) return NotFound("Teacher not found.");
            var subjects = await _context.Subjects
                .AsNoTracking()
                .Where(s => s.TeacherId == teacherId)
                .Select(s => new { s.Id, s.SubjectName })
                .ToListAsync();
            return Ok(subjects);
        }
        // ---------- 3) Enter marks by SUBJECT NAME (also upsert Grades) ----------
        // POST: /api/teacher/{teacherId}/marks
        [HttpPost("{teacherId}/marks")]
        public async Task<IActionResult> EnterMarks(int teacherId, [FromBody] MarksEntryDto dto)
        {
            if (dto == null) return BadRequest("Invalid request.");
            if (string.IsNullOrWhiteSpace(dto.SubjectName)) return BadRequest("SubjectName is required.");
            if (string.IsNullOrWhiteSpace(dto.Term)) return BadRequest("Term is required.");
            // subject must exist; also gives us the authoritative TeacherId
            var subject = await _context.Subjects
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SubjectName.ToLower() == dto.SubjectName.Trim().ToLower()
 && s.TeacherId == teacherId);
            if (subject == null)
                return NotFound("Subject not found for this teacher. Please add it first.");
            // student must exist
            var studentExists = await _context.Students.AnyAsync(s => s.Id == dto.StudentId);
            if (!studentExists) return NotFound("Student not found.");
            // create assessment (derive TeacherId from Subject to avoid FK mismatch)
            var assessment = new Assessment
            {
                StudentId = dto.StudentId,
                SubjectId = subject.Id,
                TeacherId=teacherId,
                Term = dto.Term.Trim(),
                Marks = dto.Marks,
                // include these two properties if your Assessments table has a Teacher FK:
                // TeacherId = subject.TeacherId,
                // Teacher   = null
            };
            _context.Assessments.Add(assessment);
            // derive letter grade and UPSERT (Student, Subject, Term)
            var gradeLetter = GetGradeLetter(dto.Marks);
            var existingGrade = await _context.Grades.FirstOrDefaultAsync(g =>
                g.StudentId == dto.StudentId &&
                g.SubjectId == subject.Id &&
                g.Term == dto.Term);
            if (existingGrade == null)
            {
                _context.Grades.Add(new Grade
                {
                    StudentId = dto.StudentId,
                    SubjectId = subject.Id,
                    Term = dto.Term.Trim(),
                    GradeLetter = gradeLetter
                });
            }
            else
            {
                existingGrade.GradeLetter = gradeLetter;
                _context.Grades.Update(existingGrade);
            }
            await _context.SaveChangesAsync();
            return Ok(new
            {
                assessment.Id,
                assessment.StudentId,
                SubjectId = subject.Id,
                SubjectName = subject.SubjectName,
                assessment.Term,
                assessment.Marks,
                GradeLetter = gradeLetter
            });
        }
        // ---------- 4) Mark attendance ----------
        // POST: /api/teacher/attendance
        [HttpPost("attendance")]
        public async Task<IActionResult> MarkAttendance([FromBody] AttendanceEntryDto dto)
        {
            if (dto == null) return BadRequest("Invalid request.");
            var studentExists = await _context.Students.AnyAsync(s => s.Id == dto.StudentId);
            if (!studentExists) return NotFound("Student not found.");
            var attendance = new Attendance
            {
                StudentId = dto.StudentId,
                Date = dto.Date,
                IsPresent = dto.IsPresent
            };
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                attendance.Id,
                attendance.StudentId,
                attendance.Date,
                attendance.IsPresent
            });
        }
    }
}