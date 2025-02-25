using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamic_RBAMS.Migrations
{
    /// <inheritdoc />
    public partial class CasedeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentFaculties_Departments_DepartmentId",
                table: "DepartmentFaculties");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentFaculties_DepartmentId_FacultyId",
                table: "DepartmentFaculties");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentFaculties_DepartmentId",
                table: "DepartmentFaculties",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentFaculties_Departments_DepartmentId",
                table: "DepartmentFaculties",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentFaculties_Departments_DepartmentId",
                table: "DepartmentFaculties");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentFaculties_DepartmentId",
                table: "DepartmentFaculties");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentFaculties_DepartmentId_FacultyId",
                table: "DepartmentFaculties",
                columns: new[] { "DepartmentId", "FacultyId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentFaculties_Departments_DepartmentId",
                table: "DepartmentFaculties",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
