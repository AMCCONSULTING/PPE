using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCoordinateur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Responsables_ResponsableId",
                table: "Returns");

            migrationBuilder.DropIndex(
                name: "IX_Returns_ResponsableId",
                table: "Returns");

            migrationBuilder.RenameColumn(
                name: "ResponsableId",
                table: "Returns",
                newName: "CoordinatorId");

            migrationBuilder.AddColumn<int>(
                name: "CoordinateurId",
                table: "Returns",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Returns_CoordinateurId",
                table: "Returns",
                column: "CoordinateurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Coordinateurs_CoordinateurId",
                table: "Returns",
                column: "CoordinateurId",
                principalTable: "Coordinateurs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Coordinateurs_CoordinateurId",
                table: "Returns");

            migrationBuilder.DropIndex(
                name: "IX_Returns_CoordinateurId",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "CoordinateurId",
                table: "Returns");

            migrationBuilder.RenameColumn(
                name: "CoordinatorId",
                table: "Returns",
                newName: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_ResponsableId",
                table: "Returns",
                column: "ResponsableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Responsables_ResponsableId",
                table: "Returns",
                column: "ResponsableId",
                principalTable: "Responsables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
