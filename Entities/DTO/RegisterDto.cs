
using System.ComponentModel.DataAnnotations;

namespace API.Entities.DTO
{
    public class RegisterDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public required string UserName { get; set; }

        [EmailAddress]
        [Required]
        [StringLength(255)]
        public required string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public required string Password { get; set; }
    }
}
