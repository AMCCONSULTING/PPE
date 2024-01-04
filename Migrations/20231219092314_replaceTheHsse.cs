using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class replaceTheHsse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mouvements_Hses_HseId",
                table: "Mouvements");

            migrationBuilder.RenameColumn(
                name: "HseId",
                table: "Mouvements",
                newName: "TransporteurId");

            migrationBuilder.RenameIndex(
                name: "IX_Mouvements_HseId",
                table: "Mouvements",
                newName: "IX_Mouvements_TransporteurId");

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "Stokes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Mouvements_Transporteurs_TransporteurId",
                table: "Mouvements",
                column: "TransporteurId",
                principalTable: "Transporteurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mouvements_Transporteurs_TransporteurId",
                table: "Mouvements");

            migrationBuilder.RenameColumn(
                name: "TransporteurId",
                table: "Mouvements",
                newName: "HseId");

            migrationBuilder.RenameIndex(
                name: "IX_Mouvements_TransporteurId",
                table: "Mouvements",
                newName: "IX_Mouvements_HseId");

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "Stokes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mouvements_Hses_HseId",
                table: "Mouvements",
                column: "HseId",
                principalTable: "Hses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
