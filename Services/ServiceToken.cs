using API.Entities;
using API.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class ServiceToken(IConfiguration configuration) : IServiceToken
    {
        /// <summary>
        /// Creates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user to create a token for.</param>
        /// <returns>A JWT token string.</returns>
        public string CreateToken(AppUser user)
        {
            var key = configuration["TokenKey"] ?? throw new Exception("Cannot find TokenKey in configuration");

            // HMACSHA512 requires minimum 64 bytes (128 hex characters)
            if (key.Length < 64) throw new Exception("TokenKey must be at least 64 characters long");

            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var creds = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenExpiration = configuration.GetValue<int>("JwtSettings:ExpirationHours", 2);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(tokenExpiration),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
