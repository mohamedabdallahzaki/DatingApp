using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class MembersController (DatingContext context): BaseApiController
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetAllMembers()
        {
            var users = await context.Users.ToListAsync();

            return Ok(users);
        }


        [HttpGet("{Id}")]

        public async Task<ActionResult<AppUser>> GetMemberById(string Id)
        {

            var user =await context.Users.FindAsync(Id);

            if (user == null) return NotFound();

            return Ok(user);
        }


    }
}
