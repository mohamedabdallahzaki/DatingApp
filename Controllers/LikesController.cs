using API.Entities;
using API.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController(IMemberLikeRepository likeRepository) : BaseApiController
    {
        [HttpPost("{targetMemberId}")]

        public async Task<ActionResult> ToggleLike(string targetMemberId)
        {
            var sourceMemberId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (sourceMemberId == targetMemberId)
            {
                return BadRequest("Cannot like yourself.");
            }

            var existingLike = await likeRepository.GetMemberLike(sourceMemberId, targetMemberId);

            if (existingLike == null)
            {
                var newLike = new MemberLike
                {
                    SourceMemberId = sourceMemberId,
                    TargetMemberId = targetMemberId
                };
                likeRepository.AddMemberLike(newLike);
            }
            else
            {
               likeRepository.RemoveMemberLike(existingLike);
            }

            return await likeRepository.SaveAllAsync() is true  ? Ok("Like status updated.") : BadRequest("Failed to update like status.");
        }

        [HttpGet("List")]

        public async Task<ActionResult<IReadOnlyList<string>>> GetMemberlistIds()
        {
            return Ok(await likeRepository.GetCurrentUserLikedMemberIds(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value!));
        }



        [HttpGet]

        public async Task<ActionResult<PaginatedResult<Member>>> GetMemberLikes([FromQuery] LikeParams likeParams)
        {
            likeParams.MemberId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var members = await likeRepository.GetMemberLikes(likeParams);
            return Ok(members);
        }
    }
}
