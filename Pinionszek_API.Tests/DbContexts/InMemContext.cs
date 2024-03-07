using Microsoft.EntityFrameworkCore;
using Pinionszek_API.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.DbContexts
{
    public class InMemContext
    {
        private async Task<ProdDbContext> GetDatabaseContext() 
        {
            var options = new DbContextOptionsBuilder<ProdDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var prodDbContext = new ProdDbContext(options);
            prodDbContext.Database.EnsureCreated();

            return prodDbContext;
        }
    }
}
