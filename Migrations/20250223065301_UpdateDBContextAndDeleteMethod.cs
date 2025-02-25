using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamic_RBAMS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBContextAndDeleteMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentFaculties_Faculties_FacultyId",
                table: "DepartmentFaculties");

            migrationBuilder.DropForeignKey(
                name: "FK_FacultiesCampuses_Faculties_FacultyId",
                table: "FacultiesCampuses");

            migrationBuilder.DropForeignKey(
                name: "FK_Programs_Departments_DepartmentId",
                table: "Programs");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentFaculties_Faculties_FacultyId",
                table: "DepartmentFaculties",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FacultiesCampuses_Faculties_FacultyId",
                table: "FacultiesCampuses",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_Departments_DepartmentId",
                table: "Programs",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentFaculties_Faculties_FacultyId",
                table: "DepartmentFaculties");

            migrationBuilder.DropForeignKey(
                name: "FK_FacultiesCampuses_Faculties_FacultyId",
                table: "FacultiesCampuses");

            migrationBuilder.DropForeignKey(
                name: "FK_Programs_Departments_DepartmentId",
                table: "Programs");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentFaculties_Faculties_FacultyId",
                table: "DepartmentFaculties",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacultiesCampuses_Faculties_FacultyId",
                table: "FacultiesCampuses",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_Departments_DepartmentId",
                table: "Programs",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
