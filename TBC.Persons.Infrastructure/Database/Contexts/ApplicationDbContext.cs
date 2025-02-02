using Microsoft.EntityFrameworkCore;
using TBC.Persons.Domain.Entities;

namespace TBC.Persons.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Person?> Persons { get; set; }
    public DbSet<RelatedPerson> RelatedPersons { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost\\SQLEXPRESS;Database=PersonsDb;Trusted_Connection=True;Encrypt=False;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>()
            .HasMany(rp => rp.RelatedPersons)
            .WithOne(p => p.Person)
            .HasForeignKey(rp => rp.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RelatedPerson>()
            .HasOne(rp => rp.Person)
            .WithMany(p => p.RelatedPersons)
            .HasForeignKey(rp => rp.PersonId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<RelatedPerson>()
            .HasOne(rp => rp.Person)
            .WithMany()
            .HasForeignKey(rp => rp.RelatedPersonId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Person>()
            .HasMany(x => x.PhoneNumbers);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}