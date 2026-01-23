using API.Entities;

namespace API.Interface
{
    /// <summary>
    /// Service for creating JWT authentication tokens.
    /// </summary>
    public interface IServiceToken
    {
        /// <summary>
        /// Creates a JWT token for the specified user.
        /// </summary>
        string CreateToken(AppUser user);
    }
}
