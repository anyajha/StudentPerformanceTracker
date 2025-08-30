using Microsoft.EntityFrameworkCore;
using StudentPerformanceAPI.Models;

namespace StudentPerformanceAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // User 1-1 with Student/Teacher/Admin
            modelBuilder.Entity<User>()
                .HasOne(u => u.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Teacher)
                .WithOne(t => t.User)
                .HasForeignKey<Teacher>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Admin)
                .WithOne(a => a.User)
                .HasForeignKey<Admin>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // Student → Attendance, Grades
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Attendances)
                .WithOne(a => a.Student)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Grades)
                .WithOne(g => g.Student)
                .HasForeignKey(g => g.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            // ❌ FIX HERE → Prevent multiple cascade path
            modelBuilder.Entity<Assessment>()
                .HasOne(a => a.Student)
                .WithMany(s => s.Assessments)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Restrict);  // instead of Cascade
            modelBuilder.Entity<Assessment>()
                .HasOne(a => a.Teacher)
                .WithMany(t => t.Assessments)
                .HasForeignKey(a => a.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);  // instead of Cascade

            modelBuilder.Entity<Grade>()
   .HasOne(g => g.Student)
   .WithMany(s => s.Grades)
   .HasForeignKey(g => g.StudentId)
   .OnDelete(DeleteBehavior.Restrict);   // or .NoAction()
            modelBuilder.Entity<Grade>()
               .HasOne(g => g.Subject)
               .WithMany(s => s.Grades)
               .HasForeignKey(g => g.SubjectId)
               .OnDelete(DeleteBehavior.Restrict); modelBuilder.Entity<Grade>()
               .HasOne(g => g.Student)
               .WithMany(s => s.Grades)
               .HasForeignKey(g => g.StudentId)
               .OnDelete(DeleteBehavior.Restrict);   // or .NoAction()
            modelBuilder.Entity<Grade>()
               .HasOne(g => g.Subject)
               .WithMany(s => s.Grades)
               .HasForeignKey(g => g.SubjectId)
               .OnDelete(DeleteBehavior.Restrict);


        }
    }
}