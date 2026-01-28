using API.Entities;

namespace API.Interface
{
    public interface IMemberRepository
    {
        void UpdateMember(Member member);

        Task<Member> GetMemberByIdAsync(string Id);

        Task<IReadOnlyList<Photo>> GetAllPhotosAsync(string id);
        Task<IReadOnlyList<Member>> GetAllMembersAsync();

        Task<bool> SaveAllAsync();



    }
}
