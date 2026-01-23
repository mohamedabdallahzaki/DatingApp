using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Configruation
{
    public class UserConfgruation : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasOne(u => u.AppUser)
        .WithOne(m => m.Member)
        .HasForeignKey<Member>(m => m.AppUser.Id)
        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
