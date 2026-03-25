using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignmentsToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "ClassLevel", "CreatedDate", "Points", "Subject", "Type", "UpdatedDate" },
                values: new object[] { 1, "Grade 10", new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 100m, "Mathematics", "Homework", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
