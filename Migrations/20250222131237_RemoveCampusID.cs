using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamic_RBAMS.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCampusID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faculties_Campuses_CampusId",
                table: "Faculties");

            migrationBuilder.DropIndex(
                name: "IX_Faculties_CampusId",
                table: "Faculties");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "Faculties");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
