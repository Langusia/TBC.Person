using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TBC.Persons.Domain.Entities;

namespace TBC.Persons.Infrastructure.Db.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("getdate()");

        builder.OwnsOne(x => x.Name, cfg =>
        {
            cfg.Property(x => x.Georgian)
                .HasColumnName("Name_Georgian")
                .HasColumnType("NVARCHAR(20)")
                .IsRequired();
            cfg.Property(p => p.English)
                .HasColumnName("Name_English")
                .HasColumnType("VARCHAR(20)")
                .IsRequired();
        });
    }
}