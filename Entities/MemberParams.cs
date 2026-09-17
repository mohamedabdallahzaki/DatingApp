namespace API.Entities
{
    public class MemberParams:PaginatedParams
    {
        public string? Gender { get; set; }
        public  string? currentMemberId { get; set; }
        public int maxAge { get; set; } = 99;
        public int minAge { get; set; } = 18;
        public string OrderBy { get; set; } = "lastActive";
    }
}
