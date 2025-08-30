namespace StudentPerformanceAPI.Models.DTOs
{
    public class SignupRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }   // "Student" | "Teacher" | "Admin"
        public string Name { get; set; }   // person display name
    }
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        // role-specific IDs (only one will be non-null)
        public int? StudentId { get; set; }
        public int? TeacherId { get; set; }
        public int? AdminId { get; set; }
        public string Name { get; set; }
    }
}