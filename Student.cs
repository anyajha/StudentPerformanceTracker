namespace StudentPerformanceAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public User User { get; set; }
        public ICollection<Attendance>Attendances { get; set; } 
        public ICollection<Assessment>Assessments { get; set; } 
        public ICollection<Grade> Grades { get; set; }
    }
}
