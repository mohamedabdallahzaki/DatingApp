using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DatingContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure AppUser entity
            modelBuilder.Entity<AppUser>(entity =>
            {
                // Create unique index on Email for faster lookups and uniqueness
                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_Users_Email");

                entity.HasOne(u => u.Member)
                    .WithOne(m => m.AppUser)
                    .HasForeignKey<Member>(m => m.Id)
                    .OnDelete(DeleteBehavior.Cascade);

                // Configure Email as required with max length
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                // Configure DisplayName
                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}
