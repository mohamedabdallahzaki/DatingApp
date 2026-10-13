using API.Data.Configruation;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Data
{
    public class DatingContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<MemberLike> likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfgruation).Assembly);

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

            modelBuilder.Entity<MemberLike>()
                .HasKey(k => new { k.SourceMemberId, k.TargetMemberId });


            modelBuilder.Entity<MemberLike>()
                .HasOne(s => s.SourceMember)
                .WithMany(t => t.LikedMember)
                .HasForeignKey(s => s.SourceMemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MemberLike>()
                .HasOne(l => l.TargetMember)
                .WithMany(u => u.LikedByMember)
                .HasForeignKey(l => l.TargetMemberId)
                .OnDelete(DeleteBehavior.NoAction);

            var dataTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dataTimeConverter);
                    }
                }

            }
        }
    }
}