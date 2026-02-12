using API.Data;
using API.Entities;
using API.Entities.DTO;
using API.Extensions;
using API.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    
    public class AccountController(DatingContext context, IServiceToken serviceToken) : BaseApiController
    {
       
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto register)
        {
            if (await CheckEmailExists(register.Email))
                return BadRequest("Email is already registered");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                Email = register.Email.ToLower(),
                DisplayName = register.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
                PasswordSalt = hmac.Key
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

           

            return Ok(user.ToUserDto(serviceToken));
        }

      
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == login.Email.ToLower());

            if (user == null)
                return Unauthorized("Invalid email or password");

           
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

            
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (!computedHash.SequenceEqual(user.PasswordHash))
                    return Unauthorized("Invalid email or password");
            }

            return Ok(user.ToUserDto(serviceToken));
        }

    
        private async Task<bool> CheckEmailExists(string email)
        {
            return await context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}
