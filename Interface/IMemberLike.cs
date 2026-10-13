using API.Entities;

namespace API.Interface
{
    public interface IMemberLikeRepository
    {
        Task<MemberLike?> GetMemberLike(string sourceMemberId, string targetMemberId);

        Task<IReadOnlyList<Member>> GetMemberLikes (string predicate, string memberId);

        Task<PaginatedResult<Member>> GetMemberLikes(LikeParams likeParams);

        Task<IReadOnlyList<string>> GetCurrentUserLikedMemberIds(string memberId);

        void AddMemberLike(MemberLike memberLike);

        void RemoveMemberLike(MemberLike memberLike);

        Task<bool> SaveAllAsync();
    }
}
