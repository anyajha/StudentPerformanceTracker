namespace StudentPerformanceAPI.Models.DTOs
{

    public class AttendanceEntryDto
    {
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
    }
    public class MarksEntryDto
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Term { get; set; }   // Midterm, Endterm, etc.
        public int Marks { get; set; }
    }

    public class SubjectCreateDto
    {
        
        public string Name { get; set; }
    }
}