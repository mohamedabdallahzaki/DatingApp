using API.Entities;
using API.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class MemberLikeRepository (DatingContext  context): IMemberLikeRepository
    {
        public void AddMemberLike(MemberLike memberLike)
        {
            context.likes.Add(memberLike); 
        }

        public async Task<IReadOnlyList<string>> GetCurrentUserLikedMemberIds(string memberId)
        {
            return await context.likes
                .Where(l => l.SourceMemberId == memberId)
                .Select(l => l.TargetMemberId)
                .ToListAsync();
        }

        public async Task<MemberLike?> GetMemberLike(string sourceMemberId, string targetMemberId)
        {
            return await context.likes.FindAsync(sourceMemberId, targetMemberId);
        }

        public async Task<IReadOnlyList<Member>> GetMemberLikes(string predicate, string memberId)
        {
            var query = context.likes.AsQueryable();

            switch(predicate)
            {
                case "liked":
                    return await query
                        .Where(l => l.SourceMemberId == memberId) 
                        .Select(l => l.TargetMember)
                        .ToListAsync();
                    
                case "likedBy":
                   return await query
                        .Where(l => l.TargetMemberId == memberId)
                        .Select(l => l.SourceMember).ToListAsync();  
                 
                default:

                    var likeIds = query.Where(l => l.SourceMemberId == memberId).Select(l => l.TargetMemberId);
                    return await query
                        .Where(l => l.SourceMemberId == memberId &&  likeIds.Contains(l.TargetMemberId))
                        .Select(l => l.TargetMember)
                        .ToListAsync();
            }
        }

        public async Task<PaginatedResult<Member>> GetMemberLikes(LikeParams likeParams)
        {
            var query = context.likes.AsQueryable();
            IQueryable<Member> result;

            switch(likeParams.Predicate)
            {
                case "liked":
                    result = query
                        .Where(l => l.SourceMemberId == likeParams.MemberId)
                        .Select(l => l.TargetMember);
                    break;
                case "likedBy":
                    result = query
                        .Where(l => l.TargetMemberId == likeParams.MemberId)
                        .Select(l => l.SourceMember);
                    break;
                default:
                    var ids = query.Where(l => l.SourceMemberId == likeParams.MemberId).Select(l => l.TargetMemberId);
                    result = query
                        .Where(l => l.SourceMemberId == likeParams.MemberId && ids.Contains(l.TargetMemberId))
                        .Select(l => l.TargetMember);
                    break;
            }
            return await paginatedHelper.CreateAsync(result, likeParams.PageNumber, likeParams.PageSize);
        }

        public void RemoveMemberLike(MemberLike memberLike)
        {
            context.likes.Remove(memberLike);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
