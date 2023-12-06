using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class addProjectIdAndFonctionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FunctionId",
                table: "EmployeeStocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "EmployeeStocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeStocks_FunctionId",
                table: "EmployeeStocks",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeStocks_ProjectId",
                table: "EmployeeStocks",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeStocks_Functions_FunctionId",
                table: "EmployeeStocks",
                column: "FunctionId",
                principalTable: "Functions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeStocks_Projects_ProjectId",
                table: "EmployeeStocks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeStocks_Functions_FunctionId",
                table: "EmployeeStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeStocks_Projects_ProjectId",
                table: "EmployeeStocks");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeStocks_FunctionId",
                table: "EmployeeStocks");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeStocks_ProjectId",
                table: "EmployeeStocks");

            migrationBuilder.DropColumn(
                name: "FunctionId",
                table: "EmployeeStocks");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "EmployeeStocks");
        }
    }
}
