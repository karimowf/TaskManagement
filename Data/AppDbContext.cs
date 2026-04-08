using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;

namespace TaskManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(t => t.Description)
                      .HasMaxLength(1000);

                entity.Property(t => t.Priority)
                      .IsRequired();

                entity.Property(t => t.Deadline)
                      .IsRequired(false);

                entity.Property(t => t.IsCompleted)
                      .HasDefaultValue(false);
            });
        }
    }
}
