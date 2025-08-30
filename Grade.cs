namespace StudentPerformanceAPI.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string Term { get; set; }
        public string GradeLetter { get; set; }

     
    }
}
