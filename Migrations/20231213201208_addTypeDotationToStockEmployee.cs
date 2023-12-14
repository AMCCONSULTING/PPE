using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class addTypeDotationToStockEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HseId",
                table: "EmployeeStocks");

            migrationBuilder.RenameColumn(
                name: "ResponsibleId",
                table: "EmployeeStocks",
                newName: "TypeDotation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeDotation",
                table: "EmployeeStocks",
                newName: "ResponsibleId");

            migrationBuilder.AddColumn<int>(
                name: "HseId",
                table: "EmployeeStocks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
