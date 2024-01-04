using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class dotationAndDotationDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "Mouvements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Dotation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ResponsibleId = table.Column<int>(type: "int", nullable: false),
                    CoordinatorId = table.Column<int>(type: "int", nullable: false),
                    TransporterId = table.Column<int>(type: "int", nullable: false),
                    MagasinierId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dotation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dotation_Coordinateurs_CoordinatorId",
                        column: x => x.CoordinatorId,
                        principalTable: "Coordinateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dotation_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dotation_Magaziniers_MagasinierId",
                        column: x => x.MagasinierId,
                        principalTable: "Magaziniers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dotation_Responsables_ResponsibleId",
                        column: x => x.ResponsibleId,
                        principalTable: "Responsables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dotation_Transporteurs_TransporterId",
                        column: x => x.TransporterId,
                        principalTable: "Transporteurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DotationDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DotationId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DotationDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DotationDetail_Dotation_DotationId",
                        column: x => x.DotationId,
                        principalTable: "Dotation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DotationDetail_PpeAttributeCategoryAttributeValues_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "PpeAttributeCategoryAttributeValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dotation_CoordinatorId",
                table: "Dotation",
                column: "CoordinatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Dotation_EmployeeId",
                table: "Dotation",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Dotation_MagasinierId",
                table: "Dotation",
                column: "MagasinierId");

            migrationBuilder.CreateIndex(
                name: "IX_Dotation_ResponsibleId",
                table: "Dotation",
                column: "ResponsibleId");

            migrationBuilder.CreateIndex(
                name: "IX_Dotation_TransporterId",
                table: "Dotation",
                column: "TransporterId");

            migrationBuilder.CreateIndex(
                name: "IX_DotationDetail_ArticleId",
                table: "DotationDetail",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_DotationDetail_DotationId",
                table: "DotationDetail",
                column: "DotationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DotationDetail");

            migrationBuilder.DropTable(
                name: "Dotation");

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "Mouvements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
