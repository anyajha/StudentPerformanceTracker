namespace StudentPerformanceAPI.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
        public ICollection<Subject> Subjects { get; set; }  
        public ICollection<Assessment>Assessments { get; set; }

     
    }
}
