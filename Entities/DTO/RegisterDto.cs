
using System.ComponentModel.DataAnnotations;

namespace API.Entities.DTO
{
    public class RegisterDto
    {

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public required string DisplayName { get; set; }

        [EmailAddress]
        [Required]
  
        public required string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public required string Password { get; set; }

        public required string Gender { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }
            
        [Required]

        public required string City { get; set; }

        [Required]
        public required string Country { get; set; }

    }
}
