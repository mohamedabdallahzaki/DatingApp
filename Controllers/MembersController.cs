using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController (DatingContext context): ControllerBase
    {
        [HttpGet]

        public ActionResult<IReadOnlyList<AppUser>> GetAllMembers()
        {
            var users = context.Users.ToList();

            return Ok(users);
        }


        [HttpGet("{Id}")]

        public ActionResult<AppUser> GetMemberById(string Id)
        {

            var user = context.Users.Find(Id);

            if (user == null) return NotFound();

            return Ok(user);
        }


    }
}
