using Microsoft.EntityFrameworkCore;
using TBC.Persons.Domain.Entities;

namespace TBC.Persons.Infrastructure.Localizer;

public class LocalizationDbContext : DbContext
{
    public LocalizationDbContext()
    {
    }

    public LocalizationDbContext(DbContextOptions<LocalizationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost\\SQLEXPRESS;Database=PersonsDb;Trusted_Connection=True;Encrypt=False;");
    }

    public DbSet<LocalizedString> LocalizedStrings { get; set; }
}