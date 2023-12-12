using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class addIsArchivedToEmployeeStocks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockToBePaid_EmployeeStocks_EmployeeStockId",
                table: "StockToBePaid");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockToBePaid",
                table: "StockToBePaid");

            migrationBuilder.RenameTable(
                name: "StockToBePaid",
                newName: "StocksToBePaid");

            migrationBuilder.RenameIndex(
                name: "IX_StockToBePaid_EmployeeStockId",
                table: "StocksToBePaid",
                newName: "IX_StocksToBePaid_EmployeeStockId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StocksToBePaid",
                table: "StocksToBePaid",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StocksToBePaid_EmployeeStocks_EmployeeStockId",
                table: "StocksToBePaid",
                column: "EmployeeStockId",
                principalTable: "EmployeeStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StocksToBePaid_EmployeeStocks_EmployeeStockId",
                table: "StocksToBePaid");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StocksToBePaid",
                table: "StocksToBePaid");

            migrationBuilder.RenameTable(
                name: "StocksToBePaid",
                newName: "StockToBePaid");

            migrationBuilder.RenameIndex(
                name: "IX_StocksToBePaid_EmployeeStockId",
                table: "StockToBePaid",
                newName: "IX_StockToBePaid_EmployeeStockId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockToBePaid",
                table: "StockToBePaid",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockToBePaid_EmployeeStocks_EmployeeStockId",
                table: "StockToBePaid",
                column: "EmployeeStockId",
                principalTable: "EmployeeStocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
