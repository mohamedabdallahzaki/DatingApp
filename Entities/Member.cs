using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Entities
{
    public class Member
    {
        public string Id { get; set; } = null!;

        public DateOnly DateOfBrith { get; set; }

        public required string DisplayName { get; set;  }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;


        public DateTime LastActive { get; set; } = DateTime.UtcNow;

        public string? ImageUrl { get; set; }

        public required string Gender { get; set; }

        public string? Description { get; set; }


        public required string City { get; set; }


        public required string Country { get; set; }

        [JsonIgnore]
        public List<Photo> Photos { get; set; } = [];



        // navgation prop

        [JsonIgnore]
        public string AppUserId { get; set; }

        [JsonIgnore]
        public AppUser AppUser { get; set; } = null!;       


    }
}
