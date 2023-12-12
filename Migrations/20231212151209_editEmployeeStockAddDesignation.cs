using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class editEmployeeStockAddDesignation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Designation",
                table: "EmployeeStocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PpeCondition",
                table: "EmployeeStocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StockToBePaid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeStockId = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockToBePaid", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockToBePaid_EmployeeStocks_EmployeeStockId",
                        column: x => x.EmployeeStockId,
                        principalTable: "EmployeeStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockToBePaid_EmployeeStockId",
                table: "StockToBePaid",
                column: "EmployeeStockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockToBePaid");

            migrationBuilder.DropColumn(
                name: "Designation",
                table: "EmployeeStocks");

            migrationBuilder.DropColumn(
                name: "PpeCondition",
                table: "EmployeeStocks");
        }
    }
}
