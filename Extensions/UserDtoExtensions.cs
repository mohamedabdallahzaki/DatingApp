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
        
        public static UserDto ToUserDto(this AppUser user, IServiceToken serviceToken)
        {
            return new UserDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Token = serviceToken.CreateToken(user),
                ImageUrl = user.ImageUrl,
                Email = user.Email
            };
        }

       
        public static MemberDto ToMemberDto(this AppUser user)
        {
            return new MemberDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email,
                ImageUrl = user.ImageUrl,
            };
        }
    }
}
