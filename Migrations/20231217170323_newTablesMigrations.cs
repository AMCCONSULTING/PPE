using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class newTablesMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PpeAttributeCategoryAttributeValueId = table.Column<int>(type: "int", nullable: false),
                    QuantityIn = table.Column<int>(type: "int", nullable: false),
                    QuantityOut = table.Column<int>(type: "int", nullable: false),
                    QuantityStock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainStocks_PpeAttributeCategoryAttributeValues_PpeAttributeCategoryAttributeValueId",
                        column: x => x.PpeAttributeCategoryAttributeValueId,
                        principalTable: "PpeAttributeCategoryAttributeValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    PpeAttributeCategoryAttributeValueId = table.Column<int>(type: "int", nullable: false),
                    QuantityIn = table.Column<int>(type: "int", nullable: false),
                    QuantityOut = table.Column<int>(type: "int", nullable: false),
                    QuantityStock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectStocks_PpeAttributeCategoryAttributeValues_PpeAttributeCategoryAttributeValueId",
                        column: x => x.PpeAttributeCategoryAttributeValueId,
                        principalTable: "PpeAttributeCategoryAttributeValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectStocks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MainStocks_PpeAttributeCategoryAttributeValueId",
                table: "MainStocks",
                column: "PpeAttributeCategoryAttributeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStocks_PpeAttributeCategoryAttributeValueId",
                table: "ProjectStocks",
                column: "PpeAttributeCategoryAttributeValueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStocks_ProjectId",
                table: "ProjectStocks",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MainStocks");

            migrationBuilder.DropTable(
                name: "ProjectStocks");
        }
    }
}
