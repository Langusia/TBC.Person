﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TBC.Persons.Domain.Entities;

namespace TBC.Persons.Infrastructure.Database.Configurations;

public class PhoneConfiguration : IEntityTypeConfiguration<PhoneNumber>
{
    public void Configure(EntityTypeBuilder<PhoneNumber> builder)
    {
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("getdate()");
    }
}