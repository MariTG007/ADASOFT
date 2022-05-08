using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ADASOFT.Migrations
{
    public partial class AddEnrollmentCourseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentCourses_Enrollmentes_EnrollmentId",
                table: "EnrollmentCourses");

            migrationBuilder.DropIndex(
                name: "IX_EnrollmentCourses_EnrollmentId",
                table: "EnrollmentCourses");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "EnrollmentCourses");

            migrationBuilder.DropColumn(
                name: "EnrollmentId",
                table: "EnrollmentCourses");

            migrationBuilder.DropColumn(
                name: "Schedule",
                table: "EnrollmentCourses");

            migrationBuilder.AddColumn<float>(
                name: "Quantity",
                table: "EnrollmentCourses",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "EnrollmentCourses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EnrollmentCourses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentCourses_UserId",
                table: "EnrollmentCourses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentCourses_AspNetUsers_UserId",
                table: "EnrollmentCourses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentCourses_AspNetUsers_UserId",
                table: "EnrollmentCourses");

            migrationBuilder.DropIndex(
                name: "IX_EnrollmentCourses_UserId",
                table: "EnrollmentCourses");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "EnrollmentCourses");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "EnrollmentCourses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EnrollmentCourses");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "EnrollmentCourses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EnrollmentId",
                table: "EnrollmentCourses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Schedule",
                table: "EnrollmentCourses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentCourses_EnrollmentId",
                table: "EnrollmentCourses",
                column: "EnrollmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentCourses_Enrollmentes_EnrollmentId",
                table: "EnrollmentCourses",
                column: "EnrollmentId",
                principalTable: "Enrollmentes",
                principalColumn: "Id");
        }
    }
}
