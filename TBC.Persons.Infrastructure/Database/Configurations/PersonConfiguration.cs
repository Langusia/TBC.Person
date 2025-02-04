using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TBC.Persons.Domain.Entities;

namespace TBC.Persons.Infrastructure.Database.Configurations;

public class PersonConfiguration :
    IEntityTypeConfiguration<Person>,
    IEntityTypeConfiguration<RelatedPerson>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("getdate()");
        builder
            .HasIndex(x => x.PersonalNumber)
            .IsUnique();
        builder
            .Property(x => x.PersonalNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.OwnsOne(x => x.FirstName, cfg =>
        {
            cfg.Property(x => x.Georgian)
                .HasColumnName("Firstname_Georgian")
                .HasColumnType("NVARCHAR(50)")
                .IsRequired();
            cfg.Property(p => p.English)
                .HasColumnName("Firstname_English")
                .HasColumnType("VARCHAR(50)")
                .IsRequired();
        });
        builder.OwnsOne(x => x.LastName, cfg =>
        {
            cfg.Property(x => x.Georgian)
                .HasColumnName("Lastname_Georgian")
                .HasColumnType("NVARCHAR(50)")
                .IsRequired();
            cfg.Property(p => p.English)
                .HasColumnName("Lastname_English")
                .HasColumnType("VARCHAR(50)")
                .IsRequired();
        });

        builder.HasOne(x => x.City)
            .WithMany().HasForeignKey(x => x.CityId);

        builder
            .HasMany(rp => rp.RelatedPersons)
            .WithOne(p => p.Person)
            .HasForeignKey(rp => rp.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<RelatedPerson> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("getdate()");

        builder
            .HasOne(rp => rp.Person)
            .WithMany(p => p.RelatedPersons)
            .HasForeignKey(rp => rp.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(rp => rp.Person)
            .WithMany()
            .HasForeignKey(rp => rp.RelatedPersonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}