using Microsoft.EntityFrameworkCore;
using TBC.Persons.Infrastructure.Db.Contexts;

namespace TBC.Persons.UnitTests.Fixtures;

public class InMemoryDbContextFixture : IDisposable
{
    public InMemoryDbContextFixture()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .EnableSensitiveDataLogging()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        Context = new ApplicationDbContext(options);
        Context.Database.EnsureCreated();
    }
    
    public ApplicationDbContext Context { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool disposing)
    {
        if (disposing)
        {
            Context.Dispose();
        }
    }
}