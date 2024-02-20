using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class AuditableEntityToAllEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Values",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Values",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Values",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Values",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Transporteurs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Transporteurs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Transporteurs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Transporteurs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Stokes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Stokes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StokeDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "StokeDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "StokeDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "StokeDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StocksToBePaid",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "StocksToBePaid",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "StocksToBePaid",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "StocksToBePaid",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Stocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Stocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Stocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Stocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StockEmployees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "StockEmployees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "StockEmployees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "StockEmployees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StockDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "StockDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "StockDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "StockDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Returns",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Returns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Returns",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Returns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Responsables",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Responsables",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Responsables",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Responsables",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProjectStocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ProjectStocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProjectStocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ProjectStocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Projects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Projects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Ppes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Ppes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Ppes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Ppes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PpeAttributeCategoryAttributeValues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PpeAttributeCategoryAttributeValues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PpeAttributeCategoryAttributeValues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "PpeAttributeCategoryAttributeValues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PayableStocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PayableStocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PayableStocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "PayableStocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MouvementDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MouvementDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MouvementDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "MouvementDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Managers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Managers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Managers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Managers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MainStocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MainStocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MainStocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "MainStocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Magaziniers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Magaziniers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Magaziniers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Magaziniers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Hses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Hses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Hses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Hses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Functions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Functions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EmployeeStocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EmployeeStocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "EmployeeStocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "EmployeeStocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "DotationDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "DotationDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "DotationDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "DotationDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Coordinateurs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Coordinateurs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Coordinateurs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Coordinateurs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AttrValueAttrCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AttrValueAttrCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AttrValueAttrCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AttrValueAttrCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AttributeValues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AttributeValues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AttributeValues",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AttributeValues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Attributes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Attributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Attributes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Attributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AttributeCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AttributeCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AttributeCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AttributeCategories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Transporteurs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Transporteurs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Transporteurs");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Transporteurs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StokeDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StokeDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "StokeDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "StokeDetails");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StocksToBePaid");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StocksToBePaid");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "StocksToBePaid");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "StocksToBePaid");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StockEmployees");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StockEmployees");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "StockEmployees");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "StockEmployees");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StockDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StockDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "StockDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "StockDetails");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Responsables");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Responsables");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Responsables");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Responsables");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProjectStocks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProjectStocks");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProjectStocks");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ProjectStocks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Ppes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Ppes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Ppes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Ppes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PpeAttributeCategoryAttributeValues");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PpeAttributeCategoryAttributeValues");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PpeAttributeCategoryAttributeValues");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PpeAttributeCategoryAttributeValues");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PayableStocks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PayableStocks");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PayableStocks");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PayableStocks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MouvementDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MouvementDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MouvementDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "MouvementDetails");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MainStocks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MainStocks");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MainStocks");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "MainStocks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Magaziniers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Magaziniers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Magaziniers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Magaziniers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Hses");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Hses");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Hses");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Hses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EmployeeStocks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EmployeeStocks");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "EmployeeStocks");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "EmployeeStocks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "DotationDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DotationDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "DotationDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "DotationDetails");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Coordinateurs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Coordinateurs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Coordinateurs");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Coordinateurs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AttrValueAttrCategories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AttrValueAttrCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AttrValueAttrCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AttrValueAttrCategories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AttributeValues");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AttributeValues");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AttributeValues");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AttributeValues");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AttributeCategories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AttributeCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AttributeCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AttributeCategories");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Stokes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Stokes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Functions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Functions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
