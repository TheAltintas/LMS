using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_API.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Teacher_TeacherId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentSets_Teacher_TeacherId",
                table: "AssignmentSets");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyClasses_Teacher_TeacherId",
                table: "StudyClasses");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Teacher_TeacherId",
                table: "Assignments",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentSets_Teacher_TeacherId",
                table: "AssignmentSets",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyClasses_Teacher_TeacherId",
                table: "StudyClasses",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Teacher_TeacherId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentSets_Teacher_TeacherId",
                table: "AssignmentSets");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyClasses_Teacher_TeacherId",
                table: "StudyClasses");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Teacher_TeacherId",
                table: "Assignments",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentSets_Teacher_TeacherId",
                table: "AssignmentSets",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyClasses_Teacher_TeacherId",
                table: "StudyClasses",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
