using API.Data;
using API.Entities;
using API.Entities.DTO;
using API.Extentions;
using API.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController(DatingContext context,IServiceToken
        serviceToken) : BaseApiController
    {
        [HttpPost("Register")]

        public async Task<ActionResult<AppUser>> Register(RegisterDto register)
        {
            if (await CheckEmail(register.email)) return BadRequest();

            using var hash = new HMACSHA512();
            var user = new AppUser()
            {
                Email = register.email,
                DisplayName = register.userName,
                PasswordHash = hash.ComputeHash(Encoding.UTF8.GetBytes(register.password)),
                PasswordSalt = hash.Key
            };

            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();

            return Ok(user.ToDto(serviceToken));

        }

        private async Task<bool> CheckEmail(string email)
        {
            return await context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}
