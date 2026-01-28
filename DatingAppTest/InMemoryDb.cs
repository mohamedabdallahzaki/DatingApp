using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatingAppTest
{
    public class InMemoryDb : DbContext
    {
        public DbSet<Member> Members; 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            base.OnConfiguring(optionsBuilder);
        }
    }
}
