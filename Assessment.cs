namespace StudentPerformanceAPI.Models

{

    public class Assessment

    {

        public int Id { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int TeacherId { get; set; }

        public Teacher Teacher { get; set; }

        public int SubjectId { get; set; }   // ✅ Keep as int

        public Subject Subject { get; set; } // ✅ Correct type

        public string Term { get; set; }

        public int Marks { get; set; }

        

    }

}
