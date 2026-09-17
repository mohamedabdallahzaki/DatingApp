using API.Entities;

namespace API.Interface
{
    public interface IMemberRepository
    {
        void UpdateMember(Member member);

        Task<Member> GetMemberByIdAsync(string Id);

        Task<IReadOnlyList<Photo>> GetAllPhotosAsync(string id);
        Task<PaginatedResult<Member>> GetAllMembersAsync(MemberParams memberParams);

        Task<bool> SaveAllAsync();

        Task<Member?> GetMemberForUpdate(string id);

    }
}
