using API.Entities;
using API.Entities.DTO;
using API.Interface;

namespace API.Extensions
{
    /// <summary>
    /// Extension methods for converting AppUser entities to DTOs.
    /// </summary>
    public static class UserDtoExtensions
    {
        /// <summary>
        /// Converts an AppUser entity to a UserDto with authentication token.
        /// </summary>
        public static UserDto ToUserDto(this AppUser user, IServiceToken serviceToken)
        {
            return new UserDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Token = serviceToken.CreateToken(user),
                Email = user.Email
            };
        }

        /// <summary>
        /// Converts an AppUser entity to a MemberDto (public profile).
        /// </summary>
        public static MemberDto ToMemberDto(this AppUser user)
        {
            return new MemberDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email,
                ImageUrl = null // Will be populated when image upload is implemented
            };
        }
    }
}
