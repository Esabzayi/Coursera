using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseraHomeTask.EntityClasses;

namespace CourseraHomeTask
{
    public class CourseraEntities : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<StudentCourseXref> StudentCourseXrefs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS04;Database=courseraHomeTask;Integrated Security=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
           .HasKey(s => s.PIN);

            modelBuilder.Entity<StudentCourseXref>()
                .HasKey(x => new { x.StudentPin, x.CourseId });

            modelBuilder.Entity<StudentCourseXref>()
                .HasOne(x => x.Student)
                .WithMany(s => s.CompletedCourses)
                .HasForeignKey(x => x.StudentPin);

            modelBuilder.Entity<StudentCourseXref>()
                .HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany()
                .HasForeignKey(c => c.InstructorId);
        }


    }
}
