using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamic_RBAMS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faculties_Departments_DepartmentID",
                table: "Faculties");

            migrationBuilder.DropForeignKey(
                name: "FK_Programs_Departments_DepartmentId",
                table: "Programs");

            migrationBuilder.RenameColumn(
                name: "DepartmentID",
                table: "Faculties",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Faculties_DepartmentID",
                table: "Faculties",
                newName: "IX_Faculties_DepartmentId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Programs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "Faculties",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_CampusId",
                table: "Faculties",
                column: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faculties_Campuses_CampusId",
                table: "Faculties",
                column: "CampusId",
                principalTable: "Campuses",
                principalColumn: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faculties_Departments_DepartmentId",
                table: "Faculties",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_Departments_DepartmentId",
                table: "Programs",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faculties_Campuses_CampusId",
                table: "Faculties");

            migrationBuilder.DropForeignKey(
                name: "FK_Faculties_Departments_DepartmentId",
                table: "Faculties");

            migrationBuilder.DropForeignKey(
                name: "FK_Programs_Departments_DepartmentId",
                table: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_Faculties_CampusId",
                table: "Faculties");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "Faculties");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Faculties",
                newName: "DepartmentID");

            migrationBuilder.RenameIndex(
                name: "IX_Faculties_DepartmentId",
                table: "Faculties",
                newName: "IX_Faculties_DepartmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Faculties_Departments_DepartmentID",
                table: "Faculties",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_Departments_DepartmentId",
                table: "Programs",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
