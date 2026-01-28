using API.Entities;

namespace API.Interface
{
  
    public interface IServiceToken
    {
        string CreateToken(AppUser user);
    }
}
