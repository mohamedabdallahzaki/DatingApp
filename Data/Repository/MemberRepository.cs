using API.Entities;
using API.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class MemberRepository(DatingContext context) : IMemberRepository
    {
        public async Task<IReadOnlyList<Member>> GetAllMembersAsync()
        {
            return await context.Members.ToListAsync();
        }

        public async Task<IReadOnlyList<Photo>> GetAllPhotosAsync(string id)
        {
            return await context.Members.Where(x => x.Id == id)
                .SelectMany(x => x.Photos)
                .ToListAsync();
        }

        public async Task<Member> GetMemberByIdAsync(string Id)
        {
            return await context.Members
                     
                     .FirstOrDefaultAsync(m => m.Id == Id) ?? null! ;

        }

        public async Task<Member?> GetMemberForUpdate(string id)
        {
            return await context.Members
                .Include(m => m.AppUser)
                .Include(m => m.Photos)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await  context.SaveChangesAsync() > 0;
        }

        public void UpdateMember(Member member)
        {
            context.Entry(member).State = EntityState.Modified;
        }
    }
}
