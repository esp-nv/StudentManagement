using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; } = null!;

        public DbSet<Area> Areas { get; set; } = null!;

        public DbSet<StudyProgram> StudyPrograms { get; set; } = null!;

        public DbSet<Module> Modules { get; set; } = null!;


        public DbSet<Course> Courses { get; set; } = null!;

        public DbSet<CourseOffering> CourseOfferings { get; set; } = null!;

        public DbSet<Enrollment> Enrollments { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var foreignKey in modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<CourseOffering>()
                   .ToTable("CourseOfferings", table =>
                     {
                           table.HasCheckConstraint(
                            "CK_CourseOffering_EndDate_After_StartDate",
                             "[EndDate] >= [StartDate]");
                       });

            modelBuilder.Entity<Enrollment>()
                   .HasIndex(e => new { e.StudentId, e.CourseOfferingId })
                  .IsUnique();

        }

    }
}
