using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedAssignmentEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssignedAssignmentSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfAssigned = table.Column<DateOnly>(type: "date", nullable: false),
                    Deadline = table.Column<DateOnly>(type: "date", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedAssignmentSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignedAssignmentSets_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AssignedAssignmentSets_Teacher_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teacher",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AssignedAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignedAssignmentSetId = table.Column<int>(type: "int", nullable: false),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    StudentResult = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    StudentResultFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentResultContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignedAssignments_AssignedAssignmentSets_AssignedAssignmentSetId",
                        column: x => x.AssignedAssignmentSetId,
                        principalTable: "AssignedAssignmentSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignedAssignments_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedAssignments_AssignedAssignmentSetId",
                table: "AssignedAssignments",
                column: "AssignedAssignmentSetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedAssignments_AssignmentId",
                table: "AssignedAssignments",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedAssignmentSets_StudentId",
                table: "AssignedAssignmentSets",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedAssignmentSets_TeacherId",
                table: "AssignedAssignmentSets",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignedAssignments");

            migrationBuilder.DropTable(
                name: "AssignedAssignmentSets");
        }
    }
}
