namespace API.Entities
{
    public class Photo
    {
        public int Id { get; set; }
         
        public required string ImageUrl { get; set; }

        public string? PublicId { get; set; }

        public string MemberId { get; set; }
        public  Member Member { get; set; }

    }
}
