using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ADASOFT.Migrations
{
    public partial class UntilAttendant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendantes_Cities_CityId",
                table: "Attendantes");

            migrationBuilder.DropIndex(
                name: "IX_Attendantes_CityId",
                table: "Attendantes");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Attendantes");

            migrationBuilder.CreateTable(
                name: "EnrollmentCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Schedule = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    EnrollmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnrollmentCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EnrollmentCourses_Enrollmentes_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalTable: "Enrollmentes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudenCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudenCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudenCourses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudenCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudenCourseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_StudenCourses_StudenCourseId",
                        column: x => x.StudenCourseId,
                        principalTable: "StudenCourses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FinalGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeId = table.Column<int>(type: "int", nullable: true),
                    note = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinalGrades_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentCourses_CourseId",
                table: "EnrollmentCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentCourses_EnrollmentId",
                table: "EnrollmentCourses",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentCourses_Id_CourseId",
                table: "EnrollmentCourses",
                columns: new[] { "Id", "CourseId" },
                unique: true,
                filter: "[CourseId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FinalGrades_GradeId",
                table: "FinalGrades",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalGrades_Id_GradeId",
                table: "FinalGrades",
                columns: new[] { "Id", "GradeId" },
                unique: true,
                filter: "[GradeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_Id_StudenCourseId",
                table: "Grades",
                columns: new[] { "Id", "StudenCourseId" },
                unique: true,
                filter: "[StudenCourseId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_StudenCourseId",
                table: "Grades",
                column: "StudenCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudenCourses_CourseId",
                table: "StudenCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudenCourses_Id_CourseId",
                table: "StudenCourses",
                columns: new[] { "Id", "CourseId" },
                unique: true,
                filter: "[CourseId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StudenCourses_UserId",
                table: "StudenCourses",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnrollmentCourses");

            migrationBuilder.DropTable(
                name: "FinalGrades");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "StudenCourses");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Attendantes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendantes_CityId",
                table: "Attendantes",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendantes_Cities_CityId",
                table: "Attendantes",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }
    }
}
