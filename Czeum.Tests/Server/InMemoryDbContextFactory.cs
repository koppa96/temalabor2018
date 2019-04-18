using System;
using System.Collections.Generic;
using System.Text;
using Czeum.DAL;
using Microsoft.EntityFrameworkCore;

namespace Czeum.Tests.Server
{
    public class InMemoryDbContextFactory
    {
        public ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CzeumTestDb")
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
