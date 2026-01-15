
using System.ComponentModel.DataAnnotations;

namespace API.Entities.DTO
{
    public class RegisterDto
    {
        [Required]
        public string userName { get; set; }
        [EmailAddress]
        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }
    }
}
