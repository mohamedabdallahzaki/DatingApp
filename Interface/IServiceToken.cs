using API.Entities;

namespace API.Interface
{
    public interface IServiceToken
    {
        string creatToken(AppUser user);
    }
}
