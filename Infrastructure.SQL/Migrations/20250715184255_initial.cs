using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.SQL.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Application",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Team = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TeamEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApplicationEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Application", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Associate",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Associate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Holder",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Abbreviation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "State",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Step",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    HolderId = table.Column<int>(type: "int", nullable: false),
                    IsFinal = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Step", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Step_Holder_HolderId",
                        column: x => x.HolderId,
                        principalSchema: "dbo",
                        principalTable: "Holder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Step_State_StateId",
                        column: x => x.StateId,
                        principalSchema: "dbo",
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Process",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CurrentStepId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Process", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Process_Application_ApplicationId",
                        column: x => x.ApplicationId,
                        principalSchema: "dbo",
                        principalTable: "Application",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Process_Associate_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "dbo",
                        principalTable: "Associate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Process_Step_CurrentStepId",
                        column: x => x.CurrentStepId,
                        principalSchema: "dbo",
                        principalTable: "Step",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProcessId = table.Column<int>(type: "int", nullable: false),
                    StepId = table.Column<int>(type: "int", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Document_Associate_UploadedById",
                        column: x => x.UploadedById,
                        principalSchema: "dbo",
                        principalTable: "Associate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Document_Process_ProcessId",
                        column: x => x.ProcessId,
                        principalSchema: "dbo",
                        principalTable: "Process",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Document_Step_StepId",
                        column: x => x.StepId,
                        principalSchema: "dbo",
                        principalTable: "Step",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StepHistory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessId = table.Column<int>(type: "int", nullable: false),
                    StepId = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExecutedById = table.Column<int>(type: "int", nullable: false),
                    StepEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StepHistory_Associate_ExecutedById",
                        column: x => x.ExecutedById,
                        principalSchema: "dbo",
                        principalTable: "Associate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepHistory_Process_ProcessId",
                        column: x => x.ProcessId,
                        principalSchema: "dbo",
                        principalTable: "Process",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StepHistory_Step_StepEntityId",
                        column: x => x.StepEntityId,
                        principalSchema: "dbo",
                        principalTable: "Step",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StepHistory_Step_StepId",
                        column: x => x.StepId,
                        principalSchema: "dbo",
                        principalTable: "Step",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentHistory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PerformedById = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StepId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentHistory_Associate_PerformedById",
                        column: x => x.PerformedById,
                        principalSchema: "dbo",
                        principalTable: "Associate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentHistory_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "dbo",
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentHistory_Step_StepId",
                        column: x => x.StepId,
                        principalSchema: "dbo",
                        principalTable: "Step",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Document_ProcessId",
                schema: "dbo",
                table: "Document",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_StepId",
                schema: "dbo",
                table: "Document",
                column: "StepId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_UploadedById",
                schema: "dbo",
                table: "Document",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentHistory_DocumentId",
                schema: "dbo",
                table: "DocumentHistory",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentHistory_PerformedById",
                schema: "dbo",
                table: "DocumentHistory",
                column: "PerformedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentHistory_StepId",
                schema: "dbo",
                table: "DocumentHistory",
                column: "StepId");

            migrationBuilder.CreateIndex(
                name: "IX_Process_ApplicationId",
                schema: "dbo",
                table: "Process",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Process_CreatedById",
                schema: "dbo",
                table: "Process",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Process_CurrentStepId",
                schema: "dbo",
                table: "Process",
                column: "CurrentStepId");

            migrationBuilder.CreateIndex(
                name: "IX_Process_ProcessCode",
                schema: "dbo",
                table: "Process",
                column: "ProcessCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Step_HolderId",
                schema: "dbo",
                table: "Step",
                column: "HolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Step_StateId",
                schema: "dbo",
                table: "Step",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_StepHistory_ExecutedById",
                schema: "dbo",
                table: "StepHistory",
                column: "ExecutedById");

            migrationBuilder.CreateIndex(
                name: "IX_StepHistory_ProcessId",
                schema: "dbo",
                table: "StepHistory",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_StepHistory_StepEntityId",
                schema: "dbo",
                table: "StepHistory",
                column: "StepEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_StepHistory_StepId",
                schema: "dbo",
                table: "StepHistory",
                column: "StepId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentHistory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "StepHistory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Document",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Process",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Application",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Associate",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Step",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Holder",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "State",
                schema: "dbo");
        }
    }
}
