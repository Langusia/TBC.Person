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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<LocalizedString>(x =>
        {
            x.Property(x => x.ResourceValue)
                .HasMaxLength(100);
            x.Property(x => x.ResourceKey)
                .HasMaxLength(100);
            x.Property(x => x.Culture)
                .HasMaxLength(20);
        });
    }

    public DbSet<LocalizedString> LocalizedStrings { get; set; }
}