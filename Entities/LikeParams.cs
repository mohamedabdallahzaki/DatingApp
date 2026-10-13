namespace API.Entities
{
    public class LikeParams:PaginatedParams
    {
        public  string MemberId { get; set; } = string.Empty;

        public  string Predicate { get; set; } = "like";
    }
}
