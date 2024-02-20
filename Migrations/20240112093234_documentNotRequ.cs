using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class documentNotRequ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Stokes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Stokes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Stokes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Stokes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Stokes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Stokes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Stokes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Stokes");
        }
    }
}
