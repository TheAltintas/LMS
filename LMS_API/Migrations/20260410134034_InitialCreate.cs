using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LMS_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Points = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClassLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentSets_Teacher_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudyClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyClasses_Teacher_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentAssignmentSets",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    AssignmentSetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentAssignmentSets", x => new { x.AssignmentId, x.AssignmentSetId });
                    table.ForeignKey(
                        name: "FK_AssignmentAssignmentSets_AssignmentSets_AssignmentSetId",
                        column: x => x.AssignmentSetId,
                        principalTable: "AssignmentSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentAssignmentSets_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentStudyClasses",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    StudyClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentStudyClasses", x => new { x.StudentId, x.StudyClassId });
                    table.ForeignKey(
                        name: "FK_StudentStudyClasses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentStudyClasses_StudyClasses_StudyClassId",
                        column: x => x.StudyClassId,
                        principalTable: "StudyClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "ClassLevel", "CreatedDate", "PictureUrl", "Points", "Subject", "Type", "UpdatedDate", "VideoUrl" },
                values: new object[] { 1, "A", new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/assignment1.png", 10m, "Mathematics", "Delprøve 1", null, "https://www.youtube.com/watch?v=dQw4w9WgXcQ" });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "Password", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "shoaib.ali@student.ucl.dk", "Shoaib", "Ali", "hashed_password", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "imran.khan@student.ucl.dk", "Imran", "Khan", "hashed_password", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Teacher",
                columns: new[] { "Id", "CreatedDate", "Email", "FirstName", "LastName", "Password", "UpdatedDate" },
                values: new object[] { 1, new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "morten.domsgard@ucl.dk", "Morten", "Domsgard", "1234567890", new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "AssignmentSets",
                columns: new[] { "Id", "CreatedDate", "Name", "TeacherId", "UpdatedDate" },
                values: new object[] { 1, new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Math Set 1", 1, null });

            migrationBuilder.InsertData(
                table: "StudyClasses",
                columns: new[] { "Id", "CreatedDate", "Name", "TeacherId", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Class A", 1, null },
                    { 2, new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Class B", 1, null }
                });

            migrationBuilder.InsertData(
                table: "AssignmentAssignmentSets",
                columns: new[] { "AssignmentId", "AssignmentSetId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "StudentStudyClasses",
                columns: new[] { "StudentId", "StudyClassId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentAssignmentSets_AssignmentSetId",
                table: "AssignmentAssignmentSets",
                column: "AssignmentSetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSets_TeacherId",
                table: "AssignmentSets",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentStudyClasses_StudyClassId",
                table: "StudentStudyClasses",
                column: "StudyClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyClasses_TeacherId",
                table: "StudyClasses",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentAssignmentSets");

            migrationBuilder.DropTable(
                name: "StudentStudyClasses");

            migrationBuilder.DropTable(
                name: "AssignmentSets");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "StudyClasses");

            migrationBuilder.DropTable(
                name: "Teacher");
        }
    }
}
