using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.SQL.Migrations
{
    /// <inheritdoc />
    public partial class pediram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationId1",
                schema: "dbo",
                table: "Process",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                schema: "dbo",
                table: "Process",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Process_ApplicationId1",
                schema: "dbo",
                table: "Process",
                column: "ApplicationId1");

            migrationBuilder.CreateIndex(
                name: "IX_Process_UnitId",
                schema: "dbo",
                table: "Process",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Process_Application_ApplicationId1",
                schema: "dbo",
                table: "Process",
                column: "ApplicationId1",
                principalSchema: "dbo",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Process_Unit_UnitId",
                schema: "dbo",
                table: "Process",
                column: "UnitId",
                principalSchema: "dbo",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Process_Application_ApplicationId1",
                schema: "dbo",
                table: "Process");

            migrationBuilder.DropForeignKey(
                name: "FK_Process_Unit_UnitId",
                schema: "dbo",
                table: "Process");

            migrationBuilder.DropIndex(
                name: "IX_Process_ApplicationId1",
                schema: "dbo",
                table: "Process");

            migrationBuilder.DropIndex(
                name: "IX_Process_UnitId",
                schema: "dbo",
                table: "Process");

            migrationBuilder.DropColumn(
                name: "ApplicationId1",
                schema: "dbo",
                table: "Process");

            migrationBuilder.DropColumn(
                name: "UnitId",
                schema: "dbo",
                table: "Process");
        }
    }
}
