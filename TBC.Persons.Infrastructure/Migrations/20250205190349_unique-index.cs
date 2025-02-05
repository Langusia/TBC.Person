using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TBC.Persons.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class uniqueindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumbers_Type_Number",
                table: "PhoneNumbers",
                columns: new[] { "Type", "Number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PhoneNumbers_Type_Number",
                table: "PhoneNumbers");
        }
    }
}
