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
        public string creatToken(AppUser user)
        {
            var key = configuration["TokenKey"]  ?? throw new Exception("cannn't found token key ");

            if (key.Length < 90) throw new Exception("The Lenght cann't more than 64");

            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var claims = new List<Claim>
            {
                new Claim (ClaimTypes.Email,user.Email),
                new Claim (ClaimTypes.NameIdentifier, user.Id)
            };

            var creds = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
