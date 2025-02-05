using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TBC.Persons.Infrastructure.Migrations.ReportingDb
{
    /// <inheritdoc />
    public partial class _2nd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "reporting",
                table: "RelatedPersons");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "reporting",
                table: "PhoneNumbers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "reporting",
                table: "Persons");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "reporting",
                table: "RelatedPersons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "reporting",
                table: "PhoneNumbers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "reporting",
                table: "Persons",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
