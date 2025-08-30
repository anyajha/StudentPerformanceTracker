namespace StudentPerformanceAPI.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public int TeacherId { get; set; }  

        public Teacher Teacher { get; set; }

        public ICollection<Assessment> Assessments { get; set; }
        public ICollection<Grade> Grades { get; set; }
      
    }
}
