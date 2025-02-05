﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TBC.Persons.Infrastructure.Db.Contexts;

#nullable disable

namespace TBC.Persons.Infrastructure.Migrations.ReportingDb
{
    [DbContext(typeof(ReportingDbContext))]
    partial class ReportingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("reporting")
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TBC.Persons.Domain.Entities.City", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("City", "reporting");
                });

            modelBuilder.Entity("TBC.Persons.Domain.Entities.Person", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CityId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("PersonalNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PicturePath")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("PersonalNumber")
                        .IsUnique();

                    b.ToTable("Persons", "reporting");
                });

            modelBuilder.Entity("TBC.Persons.Domain.Entities.PhoneNumber", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<long?>("PersonId")
                        .HasColumnType("bigint");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("PhoneNumbers", "reporting");
                });

            modelBuilder.Entity("TBC.Persons.Domain.Entities.RelatedPerson", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<long>("PersonId")
                        .HasColumnType("bigint");

                    b.Property<long>("RelatedPersonId")
                        .HasColumnType("bigint");

                    b.Property<int>("RelationType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("RelatedPersonId");

                    b.ToTable("RelatedPersons", "reporting");
                });

            modelBuilder.Entity("TBC.Persons.Domain.Entities.City", b =>
                {
                    b.OwnsOne("TBC.Persons.Domain.Values.MultiLanguageString", "Name", b1 =>
                        {
                            b1.Property<long>("CityId")
                                .HasColumnType("bigint");

                            b1.Property<string>("English")
                                .IsRequired()
                                .HasColumnType("VARCHAR(20)")
                                .HasColumnName("Name_English");

                            b1.Property<string>("Georgian")
                                .IsRequired()
                                .HasColumnType("NVARCHAR(20)")
                                .HasColumnName("Name_Georgian");

                            b1.HasKey("CityId");

                            b1.ToTable("City", "reporting");

                            b1.WithOwner()
                                .HasForeignKey("CityId");
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("TBC.Persons.Domain.Entities.Person", b =>
                {
                    b.HasOne("TBC.Persons.Domain.Entities.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("TBC.Persons.Domain.Values.MultiLanguageString", "FirstName", b1 =>
                        {
                            b1.Property<long>("PersonId")
                                .HasColumnType("bigint");

                            b1.Property<string>("English")
                                .IsRequired()
                                .HasColumnType("VARCHAR(50)")
                                .HasColumnName("Firstname_English");

                            b1.Property<string>("Georgian")
                                .IsRequired()
                                .HasColumnType("NVARCHAR(50)")
                                .HasColumnName("Firstname_Georgian");

                            b1.HasKey("PersonId");

                            b1.ToTable("Persons", "reporting");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");
                        });

                    b.OwnsOne("TBC.Persons.Domain.Values.MultiLanguageString", "LastName", b1 =>
                        {
                            b1.Property<long>("PersonId")
                                .HasColumnType("bigint");

                            b1.Property<string>("English")
                                .IsRequired()
                                .HasColumnType("VARCHAR(50)")
                                .HasColumnName("Lastname_English");

                            b1.Property<string>("Georgian")
                                .IsRequired()
                                .HasColumnType("NVARCHAR(50)")
                                .HasColumnName("Lastname_Georgian");

                            b1.HasKey("PersonId");

                            b1.ToTable("Persons", "reporting");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");
                        });

                    b.Navigation("City");

                    b.Navigation("FirstName")
                        .IsRequired();

                    b.Navigation("LastName")
                        .IsRequired();
                });

            modelBuilder.Entity("TBC.Persons.Domain.Entities.PhoneNumber", b =>
                {
                    b.HasOne("TBC.Persons.Domain.Entities.Person", null)
                        .WithMany("PhoneNumbers")
                        .HasForeignKey("PersonId");
                });

            modelBuilder.Entity("TBC.Persons.Domain.Entities.RelatedPerson", b =>
                {
                    b.HasOne("TBC.Persons.Domain.Entities.Person", "LinkedPerson")
                        .WithMany("RelatedPersons")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TBC.Persons.Domain.Entities.Person", "Person")
                        .WithMany()
                        .HasForeignKey("RelatedPersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("LinkedPerson");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("TBC.Persons.Domain.Entities.Person", b =>
                {
                    b.Navigation("PhoneNumbers");

                    b.Navigation("RelatedPersons");
                });
#pragma warning restore 612, 618
        }
    }
}
