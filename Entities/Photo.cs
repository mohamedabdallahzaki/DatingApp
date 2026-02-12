using System.Text.Json.Serialization;

namespace API.Entities
{
    public class Photo
    {
        public int Id { get; set; }
         
        public required string ImageUrl { get; set; }

        public string? PublicId { get; set; }
        [JsonIgnore]
        public string MemberId { get; set; }
        [JsonIgnore]
        public  Member Member { get; set; }

    }
}
