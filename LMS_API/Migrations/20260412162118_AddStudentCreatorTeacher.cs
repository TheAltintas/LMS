using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentCreatorTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByTeacherId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedByTeacherId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedByTeacherId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Students_CreatedByTeacherId",
                table: "Students",
                column: "CreatedByTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Teacher_CreatedByTeacherId",
                table: "Students",
                column: "CreatedByTeacherId",
                principalTable: "Teacher",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Teacher_CreatedByTeacherId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_CreatedByTeacherId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CreatedByTeacherId",
                table: "Students");
        }
    }
}
