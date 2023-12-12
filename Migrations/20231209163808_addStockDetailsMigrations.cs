using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class addStockDetailsMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttributeCategory_Attributes_AttributeId",
                table: "AttributeCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_AttributeCategory_Categories_CategoryId",
                table: "AttributeCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_AttrValueAttrCategories_AttributeCategory_AttributeCategoryId",
                table: "AttrValueAttrCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Projects_ProjectId",
                table: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AttributeCategory",
                table: "AttributeCategories");

            migrationBuilder.RenameTable(
                name: "AttributeCategories",
                newName: "AttributeCategories");

            migrationBuilder.RenameIndex(
                name: "IX_AttributeCategory_CategoryId",
                table: "AttributeCategories",
                newName: "IX_AttributeCategories_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_AttributeCategory_AttributeId",
                table: "AttributeCategories",
                newName: "IX_AttributeCategories_AttributeId");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Stocks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PpeId",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StockNature",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttributeCategories",
                table: "AttributeCategories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PpeAttributeCategoryAttributeValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PpeId = table.Column<int>(type: "int", nullable: false),
                    AttributeValueAttributeCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PpeAttributeCategoryAttributeValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PpeAttributeCategoryAttributeValues_AttrValueAttrCategories_AttributeValueAttributeCategoryId",
                        column: x => x.AttributeValueAttributeCategoryId,
                        principalTable: "AttrValueAttrCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    PpeAttributeCategoryAttributeValueId = table.Column<int>(type: "int", nullable: false),
                    StockIn = table.Column<int>(type: "int", nullable: false),
                    StockOut = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockDetails_PpeAttributeCategoryAttributeValues_PpeAttributeCategoryAttributeValueId",
                        column: x => x.PpeAttributeCategoryAttributeValueId,
                        principalTable: "PpeAttributeCategoryAttributeValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockDetails_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_PpeId",
                table: "Stocks",
                column: "PpeId");

            migrationBuilder.CreateIndex(
                name: "IX_PpeAttributeCategoryAttributeValues_AttributeValueAttributeCategoryId",
                table: "PpeAttributeCategoryAttributeValues",
                column: "AttributeValueAttributeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StockDetails_PpeAttributeCategoryAttributeValueId",
                table: "StockDetails",
                column: "PpeAttributeCategoryAttributeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_StockDetails_StockId",
                table: "StockDetails",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeCategories_Attributes_AttributeId",
                table: "AttributeCategories",
                column: "AttributeId",
                principalTable: "Attributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeCategories_Categories_CategoryId",
                table: "AttributeCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttrValueAttrCategories_AttributeCategories_AttributeCategoryId",
                table: "AttrValueAttrCategories",
                column: "AttributeCategoryId",
                principalTable: "AttributeCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Ppes_PpeId",
                table: "Stocks",
                column: "PpeId",
                principalTable: "Ppes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Projects_ProjectId",
                table: "Stocks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttributeCategories_Attributes_AttributeId",
                table: "AttributeCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_AttributeCategories_Categories_CategoryId",
                table: "AttributeCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_AttrValueAttrCategories_AttributeCategories_AttributeCategoryId",
                table: "AttrValueAttrCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Ppes_PpeId",
                table: "Stocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Projects_ProjectId",
                table: "Stocks");

            migrationBuilder.DropTable(
                name: "StockDetails");

            migrationBuilder.DropTable(
                name: "PpeAttributeCategoryAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_PpeId",
                table: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AttributeCategories",
                table: "AttributeCategories");

            migrationBuilder.DropColumn(
                name: "PpeId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "StockNature",
                table: "Stocks");

            migrationBuilder.RenameTable(
                name: "AttributeCategories",
                newName: "AttributeCategories");

            migrationBuilder.RenameIndex(
                name: "IX_AttributeCategories_CategoryId",
                table: "AttributeCategories",
                newName: "IX_AttributeCategory_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_AttributeCategories_AttributeId",
                table: "AttributeCategories",
                newName: "IX_AttributeCategory_AttributeId");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttributeCategory",
                table: "AttributeCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeCategory_Attributes_AttributeId",
                table: "AttributeCategories",
                column: "AttributeId",
                principalTable: "Attributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeCategory_Categories_CategoryId",
                table: "AttributeCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttrValueAttrCategories_AttributeCategory_AttributeCategoryId",
                table: "AttrValueAttrCategories",
                column: "AttributeCategoryId",
                principalTable: "AttributeCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Projects_ProjectId",
                table: "Stocks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
