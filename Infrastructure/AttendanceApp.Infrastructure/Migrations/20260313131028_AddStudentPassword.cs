using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Students");
        }
    }
}
