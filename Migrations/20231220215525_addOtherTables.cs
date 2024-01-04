using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class addOtherTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayableStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayableStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayableStocks_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayableStocks_PpeAttributeCategoryAttributeValues_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "PpeAttributeCategoryAttributeValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Returns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    ResponsableId = table.Column<int>(type: "int", nullable: false),
                    HseId = table.Column<int>(type: "int", nullable: true),
                    MagazinierId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Returns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Returns_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Returns_Hses_HseId",
                        column: x => x.HseId,
                        principalTable: "Hses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Returns_Magaziniers_MagazinierId",
                        column: x => x.MagazinierId,
                        principalTable: "Magaziniers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Returns_PpeAttributeCategoryAttributeValues_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "PpeAttributeCategoryAttributeValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Returns_Responsables_ResponsableId",
                        column: x => x.ResponsableId,
                        principalTable: "Responsables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockEmployees_PpeAttributeCategoryAttributeValues_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "PpeAttributeCategoryAttributeValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayableStocks_ArticleId",
                table: "PayableStocks",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_PayableStocks_EmployeeId",
                table: "PayableStocks",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_ArticleId",
                table: "Returns",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_EmployeeId",
                table: "Returns",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_HseId",
                table: "Returns",
                column: "HseId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_MagazinierId",
                table: "Returns",
                column: "MagazinierId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_ResponsableId",
                table: "Returns",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_StockEmployees_ArticleId",
                table: "StockEmployees",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_StockEmployees_EmployeeId",
                table: "StockEmployees",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayableStocks");

            migrationBuilder.DropTable(
                name: "Returns");

            migrationBuilder.DropTable(
                name: "StockEmployees");
        }
    }
}
