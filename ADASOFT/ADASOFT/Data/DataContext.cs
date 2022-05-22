using ADASOFT.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ADASOFT.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Course> Courses{ get; set; }
        public DbSet<Attendant> Attendantes { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<EnrollmentCourse> EnrollmentCourses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<FinalGrade> FinalGrades { get; set; }
        public DbSet<CourseImage> CourseImages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<State>().HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<City>().HasIndex("Name", "StateId").IsUnique(); 
            modelBuilder.Entity<Campus>().HasIndex("Name", "CityId").IsUnique();
            modelBuilder.Entity<Attendant>().HasIndex("Document", "UserId").IsUnique();
            modelBuilder.Entity<Enrollment>().HasIndex("Id", "UserId").IsUnique();
            modelBuilder.Entity<EnrollmentCourse>().HasIndex("Id", "CourseId").IsUnique();
            modelBuilder.Entity<StudentCourse>().HasIndex("Id", "CourseId").IsUnique();
            modelBuilder.Entity<Grade>().HasIndex("Id", "StudentCourseId").IsUnique();
            modelBuilder.Entity<FinalGrade>().HasIndex("Id", "GradeId").IsUnique();
        }
    }
}