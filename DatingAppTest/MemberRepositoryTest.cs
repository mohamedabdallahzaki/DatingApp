using API.Data;
using API.Data.Repository;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatingAppTest
{
    public class MemberRepositoryTest
    {
        [Fact]
        public async Task Test_GetMmberBy_ID_And_Return_Member()
        {
            //arrange

            var options = new DbContextOptionsBuilder<DatingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            using var context = new DatingContext(options);

            context.Members.Add(new Member
            {
                Id = "lisa-id",
                AppUserId = "user1",
                DisplayName = "Lisa",
                City = "Cairo",
                Country = "EG",
                DateOfBrith = new DateOnly(2001, 3, 12),
                CreateAt = DateTime.UtcNow,
                Gender = "F",
                Description = "Test member"
            });

            await context.SaveChangesAsync();


            var repo = new MemberRepository(context);

            var res = await repo.GetMemberByIdAsync("lisa-id");


            Assert.NotNull(res);
            Assert.Equal("Lisa", res.DisplayName);










        }
    }
}
