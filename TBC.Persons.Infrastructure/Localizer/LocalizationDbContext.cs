using Microsoft.EntityFrameworkCore;
using TBC.Persons.Domain.Entities;

namespace TBC.Persons.Infrastructure.Localizer;

public class LocalizationDbContext : DbContext
{
    public LocalizationDbContext(DbContextOptions<LocalizationDbContext> options) : base(options)
    {
    }

    public DbSet<LocalizedString> LocalizedStrings { get; set; }
}