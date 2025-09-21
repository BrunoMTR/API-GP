using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.SQL.Migrations
{
    /// <inheritdoc />
    public partial class Documentationcorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documentation_Process_ProcessId",
                schema: "dbo",
                table: "Documentation");

            migrationBuilder.AddForeignKey(
                name: "FK_Documentation_Process_ProcessId",
                schema: "dbo",
                table: "Documentation",
                column: "ProcessId",
                principalSchema: "dbo",
                principalTable: "Process",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documentation_Process_ProcessId",
                schema: "dbo",
                table: "Documentation");

            migrationBuilder.AddForeignKey(
                name: "FK_Documentation_Process_ProcessId",
                schema: "dbo",
                table: "Documentation",
                column: "ProcessId",
                principalSchema: "dbo",
                principalTable: "Process",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
