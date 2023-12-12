using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class addStockTypeToStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockType",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PpeAttributeCategoryAttributeValues_PpeId",
                table: "PpeAttributeCategoryAttributeValues",
                column: "PpeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PpeAttributeCategoryAttributeValues_Ppes_PpeId",
                table: "PpeAttributeCategoryAttributeValues",
                column: "PpeId",
                principalTable: "Ppes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PpeAttributeCategoryAttributeValues_Ppes_PpeId",
                table: "PpeAttributeCategoryAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_PpeAttributeCategoryAttributeValues_PpeId",
                table: "PpeAttributeCategoryAttributeValues");

            migrationBuilder.DropColumn(
                name: "StockType",
                table: "Stocks");
        }
    }
}
