using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Configruation
{
    public class UserConfgruation : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
         builder.HasOne(u => u.Member)
        .WithOne(m => m.AppUser)
        .HasForeignKey<Member>(m => m.Id)
        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
