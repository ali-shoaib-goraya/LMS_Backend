using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamic_RBAMS.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDepartmentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faculties_Departments_DepartmentId",
                table: "Faculties");

            migrationBuilder.DropIndex(
                name: "IX_Faculties_DepartmentId",
                table: "Faculties");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Faculties");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Faculties",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_DepartmentId",
                table: "Faculties",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faculties_Departments_DepartmentId",
                table: "Faculties",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId");
        }
    }
}
