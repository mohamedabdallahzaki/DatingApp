using API.Entities;
using API.Entities.DTO;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace API.Data
{
    public static class SeedData
    {
        public static async Task DataSeeding (DatingContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var memberData = await File.ReadAllTextAsync("Data/UserSeedData.json");


            var members = JsonSerializer.Deserialize<List<UserSeedDto>>(memberData);

            if (members == null) {
                Console.WriteLine("Not data get from file userseeddata.json");
            return;
            }

            foreach(var member in members)
            {
            using var hmac = new HMACSHA512();
                var user = new AppUser
                {
                    Id = member.Id,
                    DisplayName = member.DisplayName,
                    ImageUrl = member.ImageUrl,
                    Email = member.Email,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("pa$$w0rd")),
                    PasswordSalt = hmac.Key,
                };

                var newMember = new Member
                {
                    Id = member.Id,
                    Country = member.Country,
                    City = member.City,
                    DisplayName = member.DisplayName,
                    CreateAt = member.Created,
                    LastActive = member.LastActive,
                    DateOfBirth = member.DateOfBirth,
                    Gender = member.Gender,
                    Description = member.Description,
                    ImageUrl = member.ImageUrl,
            

                };
                user.Member = newMember;

                await context.Members.AddAsync(newMember);
                user.Member.Photos.Add(new Photo
                {
                    ImageUrl = member.ImageUrl!,
                    MemberId = member.Id

                });
               await context.Users.AddAsync(user);
            }

            await context.SaveChangesAsync();
        }
    }
}
