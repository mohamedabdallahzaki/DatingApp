using API.Data;
using API.Entities;
using API.Entities.DTO;
using API.Extensions;
using API.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    /// <summary>
    /// Manages member profiles and user data.
    /// </summary>
    [Authorize]
    public class MembersController(IMemberRepository _memberRepository, IPhotoService _photoService) : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetAllMembers()
        {


            return Ok(await _memberRepository.GetAllMembersAsync());
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

        [HttpPut]

        public async Task<ActionResult> UpdateMember(UpdateMebmerDto updateMebmer)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null) return BadRequest("Opus , Not Found Id for this member");
            var member = await _memberRepository.GetMemberForUpdate(id);

            if (member == null) return BadRequest("Not Found Member ");

            member.Description = updateMebmer.Description ?? member.Description;
            member.City = updateMebmer.City ?? member.City;
            member.Country = updateMebmer.Country ?? member.Country;
            member.DisplayName = updateMebmer.DisplayName ?? member.DisplayName;

            member.AppUser.DisplayName = updateMebmer.DisplayName ?? member.DisplayName;

            _memberRepository.UpdateMember(member);

            if (await _memberRepository.SaveAllAsync()) return NoContent();

            return BadRequest("No change in Member");

        }
        [HttpPost("add-photo")]

        public async Task<ActionResult<Photo>> AddPhoto([FromForm] IFormFile File)
        {
            var member = await _memberRepository.GetMemberForUpdate(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (member == null) return BadRequest("cann't found member to update");

            var result = await _photoService.UploadIamge(File);

            if (result.Error != null) return BadRequest(result.Error.Message);


            var photo = new Photo
            {
                ImageUrl = result.SecureUrl.AbsoluteUri,
                MemberId = member.Id,
                PublicId = result.PublicId
            };

            if (member.ImageUrl == null)
            {
                member.ImageUrl = photo.ImageUrl;
                member.AppUser.ImageUrl = photo.ImageUrl;
            }

            member.Photos.Add(photo);

            await _memberRepository.SaveAllAsync();

            return Ok(photo);


        }


        [HttpPost("set-main-photo/{photoId}")]

        public async Task<ActionResult> SetMainPhoto( int photoId)
        {
            var member = await _memberRepository.GetMemberForUpdate(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (member == null) return BadRequest("cann't found member to update");
            var photo = member.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null || member.ImageUrl == photo.ImageUrl) return BadRequest("cann't found photo to set as main");
            member.ImageUrl = photo.ImageUrl;
            member.AppUser.ImageUrl = photo.ImageUrl;
            await _memberRepository.SaveAllAsync();
            return NoContent();
        }

        [HttpDelete("delete-photo/{photoId}")]

        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var member = await _memberRepository.GetMemberForUpdate(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (member == null) return BadRequest("cann't found member to update");
            var photo = member.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return BadRequest("cann't found photo to delete");
            if (member.ImageUrl == photo.ImageUrl) return BadRequest("cann't delete main photo");
            var result = await _photoService.DeleteImage(photo.PublicId!);
            if (result.Error != null) return BadRequest(result.Error.Message);
            member.Photos.Remove(photo);
            await _memberRepository.SaveAllAsync();
            return NoContent();
        }
    }
}