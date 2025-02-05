using Microsoft.EntityFrameworkCore;
using TBC.Persons.Domain.Entities;

namespace TBC.Persons.Infrastructure.Db.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public ApplicationDbContext()
    {
    }

    public DbSet<Person?> Persons { get; set; }
    public DbSet<RelatedPerson> RelatedPersons { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                    "Server=localhost\\SQLEXPRESS;Database=PersonsDb;Trusted_Connection=True;Encrypt=False;")
                .UseLazyLoadingProxies();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}