using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class addCreateAtAndUpdatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Mouvements",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Mouvements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Mouvements",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Mouvements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Functions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Functions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Functions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Functions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Dotations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Dotations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Dotations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Dotations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Mouvements");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Mouvements");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Mouvements");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Mouvements");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Functions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Functions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Functions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Functions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Dotations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Dotations");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Dotations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Dotations");
        }
    }
}
