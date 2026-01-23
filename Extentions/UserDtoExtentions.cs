using API.Entities;
using API.Entities.DTO;
using API.Interface;

namespace API.Extentions
{
    public static class UserDtoExtentions
    {
        public static UserDto ToDto(this AppUser user, IServiceToken serviceToken)
        {
            return new UserDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Token = serviceToken.CreateToken(user),
                Email = user.Email
            };
        }
    }
}
