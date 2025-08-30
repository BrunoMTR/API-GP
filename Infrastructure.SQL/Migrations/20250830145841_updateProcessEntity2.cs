using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.SQL.Migrations
{
    /// <inheritdoc />
    public partial class updateProcessEntity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessEntity_Application_ApplicationId",
                table: "ProcessEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessEntity_Unit_At",
                table: "ProcessEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessEntity",
                table: "ProcessEntity");

            migrationBuilder.RenameTable(
                name: "ProcessEntity",
                newName: "Process",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessEntity_At",
                schema: "dbo",
                table: "Process",
                newName: "IX_Process_At");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessEntity_ApplicationId",
                schema: "dbo",
                table: "Process",
                newName: "IX_Process_ApplicationId");

            migrationBuilder.AlterColumn<int>(
                name: "Approvals",
                table: "Node",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Process",
                schema: "dbo",
                table: "Process",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Process_Application_ApplicationId",
                schema: "dbo",
                table: "Process",
                column: "ApplicationId",
                principalSchema: "dbo",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Process_Unit_At",
                schema: "dbo",
                table: "Process",
                column: "At",
                principalSchema: "dbo",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Process_Application_ApplicationId",
                schema: "dbo",
                table: "Process");

            migrationBuilder.DropForeignKey(
                name: "FK_Process_Unit_At",
                schema: "dbo",
                table: "Process");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Process",
                schema: "dbo",
                table: "Process");

            migrationBuilder.RenameTable(
                name: "Process",
                schema: "dbo",
                newName: "ProcessEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Process_At",
                table: "ProcessEntity",
                newName: "IX_ProcessEntity_At");

            migrationBuilder.RenameIndex(
                name: "IX_Process_ApplicationId",
                table: "ProcessEntity",
                newName: "IX_ProcessEntity_ApplicationId");

            migrationBuilder.AlterColumn<int>(
                name: "Approvals",
                table: "Node",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessEntity",
                table: "ProcessEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessEntity_Application_ApplicationId",
                table: "ProcessEntity",
                column: "ApplicationId",
                principalSchema: "dbo",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessEntity_Unit_At",
                table: "ProcessEntity",
                column: "At",
                principalSchema: "dbo",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
