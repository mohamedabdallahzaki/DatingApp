using API.Data;
using API.Entities.DTO;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    /// <summary>
    /// Manages member profiles and user data.
    /// </summary>
    [Authorize]
    public class MembersController(DatingContext context) : BaseApiController
    {
        /// <summary>
        /// Gets all members with pagination support.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MemberDto>>> GetAllMembers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            // Validate pagination parameters
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 50) pageSize = 50; // Max page size

            var users = await context.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => u.ToMemberDto())
                .ToListAsync();

            return Ok(users);
        }

        /// <summary>
        /// Gets a specific member by their ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetMemberById(string id)
        {
            var user = await context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user.ToMemberDto());
        }
    }
}
