using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ADASOFT.Migrations
{
    public partial class AddFinalGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalGrades");

            migrationBuilder.AddColumn<decimal>(
                name: "FinalGrade",
                table: "StudentCourses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalGrade",
                table: "Grades",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalGrade",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "FinalGrade",
                table: "Grades");

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
                name: "IX_FinalGrades_GradeId",
                table: "FinalGrades",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalGrades_Id_GradeId",
                table: "FinalGrades",
                columns: new[] { "Id", "GradeId" },
                unique: true,
                filter: "[GradeId] IS NOT NULL");
        }
    }
}
