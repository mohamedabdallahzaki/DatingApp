using API.Data;
using API.Entities;
using API.Entities.DTO;
using API.Extensions;
using API.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    /// <summary>
    /// Manages member profiles and user data.
    /// </summary>
    [Authorize]
    public class MembersController(IMemberRepository _memberRepository) : BaseApiController
    {
        
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MemberDto>>> GetAllMembers()
        {
           

            return Ok( await _memberRepository.GetAllMembersAsync());
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMemberById(string id)
        {
            var user = await _memberRepository.GetMemberByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("{id}/Photos")]

        public async Task<ActionResult<IReadOnlyList<Photo>>> GetAllPhotoMember(string id)
        {
            var photos = await _memberRepository.GetAllPhotosAsync(id);

            if (photos == null) return NotFound();

            return Ok(photos);
        }
    }
}
