using API.Entities;
using API.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class MemberRepository(DatingContext context) : IMemberRepository
    {
        public async Task<PaginatedResult<Member>> GetAllMembersAsync(MemberParams _params)
        {
            var query = context.Members.AsQueryable();

           query = query.Where(m => m.Id != _params.currentMemberId);

            if (!string.IsNullOrEmpty(_params.Gender))
            {
                query = query.Where(m => m.Gender == _params.Gender);
            }

            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-_params.maxAge));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-_params.minAge));

            query = query.Where(m => m.DateOfBirth >= minDob && m.DateOfBirth <= maxDob);

            query = _params.OrderBy switch
            {
                "created" => query.OrderByDescending(m => m.CreateAt),
                _ => query.OrderByDescending(m => m.LastActive)
            };

            return  await paginatedHelper.CreateAsync(query, _params.PageNumber, _params.PageSize);
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
