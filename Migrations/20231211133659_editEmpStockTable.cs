using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class editEmpStockTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VariantValueId",
                table: "EmployeeStocks",
                newName: "StockType");

            migrationBuilder.AddColumn<int>(
                name: "PpeAttributeCategoryAttributeValueId",
                table: "EmployeeStocks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeStocks_PpeAttributeCategoryAttributeValueId",
                table: "EmployeeStocks",
                column: "PpeAttributeCategoryAttributeValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeStocks_PpeAttributeCategoryAttributeValues_PpeAttributeCategoryAttributeValueId",
                table: "EmployeeStocks",
                column: "PpeAttributeCategoryAttributeValueId",
                principalTable: "PpeAttributeCategoryAttributeValues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeStocks_PpeAttributeCategoryAttributeValues_PpeAttributeCategoryAttributeValueId",
                table: "EmployeeStocks");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeStocks_PpeAttributeCategoryAttributeValueId",
                table: "EmployeeStocks");

            migrationBuilder.DropColumn(
                name: "PpeAttributeCategoryAttributeValueId",
                table: "EmployeeStocks");

            migrationBuilder.RenameColumn(
                name: "StockType",
                table: "EmployeeStocks",
                newName: "VariantValueId");
        }
    }
}
