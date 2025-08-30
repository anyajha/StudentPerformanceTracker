namespace StudentPerformanceAPI.Models.DTOs
{
   
   public class MarksViewDto
    {
        public string SubjectName { get; set; }
        public string Term { get; set; }
        public int Marks { get; set; }
    }
    public class AttendanceViewDto
    {
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
    }
}