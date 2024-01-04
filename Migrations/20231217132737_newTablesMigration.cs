using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class newTablesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coordinateurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinateurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coordinateurs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hses_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Magaziniers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Magaziniers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Magaziniers_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Responsables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responsables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responsables_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transporteurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transporteurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transporteurs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mouvements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ResponsableId = table.Column<int>(type: "int", nullable: false),
                    MagazinierId = table.Column<int>(type: "int", nullable: false),
                    HseId = table.Column<int>(type: "int", nullable: false),
                    CoordinateurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mouvements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mouvements_Coordinateurs_CoordinateurId",
                        column: x => x.CoordinateurId,
                        principalTable: "Coordinateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mouvements_Hses_HseId",
                        column: x => x.HseId,
                        principalTable: "Hses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mouvements_Magaziniers_MagazinierId",
                        column: x => x.MagazinierId,
                        principalTable: "Magaziniers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mouvements_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mouvements_Responsables_ResponsableId",
                        column: x => x.ResponsableId,
                        principalTable: "Responsables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stokes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MagazinierId = table.Column<int>(type: "int", nullable: true),
                    ResponsableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stokes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stokes_Magaziniers_MagazinierId",
                        column: x => x.MagazinierId,
                        principalTable: "Magaziniers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stokes_Responsables_ResponsableId",
                        column: x => x.ResponsableId,
                        principalTable: "Responsables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MouvementDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MouvementId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MouvementDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MouvementDetails_Mouvements_MouvementId",
                        column: x => x.MouvementId,
                        principalTable: "Mouvements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MouvementDetails_PpeAttributeCategoryAttributeValues_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "PpeAttributeCategoryAttributeValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StokeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StokeId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StokeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StokeDetails_PpeAttributeCategoryAttributeValues_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "PpeAttributeCategoryAttributeValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StokeDetails_Stokes_StokeId",
                        column: x => x.StokeId,
                        principalTable: "Stokes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coordinateurs_EmployeeId",
                table: "Coordinateurs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Hses_EmployeeId",
                table: "Hses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Magaziniers_EmployeeId",
                table: "Magaziniers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_MouvementDetails_ArticleId",
                table: "MouvementDetails",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MouvementDetails_MouvementId",
                table: "MouvementDetails",
                column: "MouvementId");

            migrationBuilder.CreateIndex(
                name: "IX_Mouvements_CoordinateurId",
                table: "Mouvements",
                column: "CoordinateurId");

            migrationBuilder.CreateIndex(
                name: "IX_Mouvements_HseId",
                table: "Mouvements",
                column: "HseId");

            migrationBuilder.CreateIndex(
                name: "IX_Mouvements_MagazinierId",
                table: "Mouvements",
                column: "MagazinierId");

            migrationBuilder.CreateIndex(
                name: "IX_Mouvements_ProjectId",
                table: "Mouvements",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Mouvements_ResponsableId",
                table: "Mouvements",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_Responsables_EmployeeId",
                table: "Responsables",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_StokeDetails_ArticleId",
                table: "StokeDetails",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_StokeDetails_StokeId",
                table: "StokeDetails",
                column: "StokeId");

            migrationBuilder.CreateIndex(
                name: "IX_Stokes_MagazinierId",
                table: "Stokes",
                column: "MagazinierId");

            migrationBuilder.CreateIndex(
                name: "IX_Stokes_ResponsableId",
                table: "Stokes",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_Transporteurs_EmployeeId",
                table: "Transporteurs",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MouvementDetails");

            migrationBuilder.DropTable(
                name: "StokeDetails");

            migrationBuilder.DropTable(
                name: "Transporteurs");

            migrationBuilder.DropTable(
                name: "Mouvements");

            migrationBuilder.DropTable(
                name: "Stokes");

            migrationBuilder.DropTable(
                name: "Coordinateurs");

            migrationBuilder.DropTable(
                name: "Hses");

            migrationBuilder.DropTable(
                name: "Magaziniers");

            migrationBuilder.DropTable(
                name: "Responsables");
        }
    }
}
