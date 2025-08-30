namespace StudentPerformanceAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Student, Teacher, Admin

        public Student Student { get; set; }
        public Teacher Teacher { get; set; }
        public Admin   Admin { get; set; }
       
    }
}
