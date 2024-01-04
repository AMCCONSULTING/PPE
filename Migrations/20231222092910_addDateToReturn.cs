using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class addDateToReturn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Magaziniers_MagazinierId",
                table: "Returns");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "StocksToBePaid",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MagazinierId",
                table: "Returns",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Returns",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_StocksToBePaid_EmployeeId",
                table: "StocksToBePaid",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Magaziniers_MagazinierId",
                table: "Returns",
                column: "MagazinierId",
                principalTable: "Magaziniers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StocksToBePaid_Employees_EmployeeId",
                table: "StocksToBePaid",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Magaziniers_MagazinierId",
                table: "Returns");

            migrationBuilder.DropForeignKey(
                name: "FK_StocksToBePaid_Employees_EmployeeId",
                table: "StocksToBePaid");

            migrationBuilder.DropIndex(
                name: "IX_StocksToBePaid_EmployeeId",
                table: "StocksToBePaid");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "StocksToBePaid");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Returns");

            migrationBuilder.AlterColumn<int>(
                name: "MagazinierId",
                table: "Returns",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Magaziniers_MagazinierId",
                table: "Returns",
                column: "MagazinierId",
                principalTable: "Magaziniers",
                principalColumn: "Id");
        }
    }
}
