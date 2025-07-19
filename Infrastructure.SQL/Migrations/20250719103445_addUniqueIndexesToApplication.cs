using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.SQL.Migrations
{
    /// <inheritdoc />
    public partial class addUniqueIndexesToApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Application_Abbreviation",
                schema: "dbo",
                table: "Application",
                column: "Abbreviation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Application_Name",
                schema: "dbo",
                table: "Application",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Application_Abbreviation",
                schema: "dbo",
                table: "Application");

            migrationBuilder.DropIndex(
                name: "IX_Application_Name",
                schema: "dbo",
                table: "Application");
        }
    }
}
