using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPE.Migrations
{
    /// <inheritdoc />
    public partial class dotationAndDotationDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dotation_Coordinateurs_CoordinatorId",
                table: "Dotation");

            migrationBuilder.DropForeignKey(
                name: "FK_Dotation_Employees_EmployeeId",
                table: "Dotation");

            migrationBuilder.DropForeignKey(
                name: "FK_Dotation_Magaziniers_MagasinierId",
                table: "Dotation");

            migrationBuilder.DropForeignKey(
                name: "FK_Dotation_Responsables_ResponsibleId",
                table: "Dotation");

            migrationBuilder.DropForeignKey(
                name: "FK_Dotation_Transporteurs_TransporterId",
                table: "Dotation");

            migrationBuilder.DropForeignKey(
                name: "FK_DotationDetail_Dotation_DotationId",
                table: "DotationDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DotationDetail_PpeAttributeCategoryAttributeValues_ArticleId",
                table: "DotationDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DotationDetail",
                table: "DotationDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dotation",
                table: "Dotation");

            migrationBuilder.RenameTable(
                name: "DotationDetail",
                newName: "DotationDetails");

            migrationBuilder.RenameTable(
                name: "Dotation",
                newName: "Dotations");

            migrationBuilder.RenameIndex(
                name: "IX_DotationDetail_DotationId",
                table: "DotationDetails",
                newName: "IX_DotationDetails_DotationId");

            migrationBuilder.RenameIndex(
                name: "IX_DotationDetail_ArticleId",
                table: "DotationDetails",
                newName: "IX_DotationDetails_ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_Dotation_TransporterId",
                table: "Dotations",
                newName: "IX_Dotations_TransporterId");

            migrationBuilder.RenameIndex(
                name: "IX_Dotation_ResponsibleId",
                table: "Dotations",
                newName: "IX_Dotations_ResponsibleId");

            migrationBuilder.RenameIndex(
                name: "IX_Dotation_MagasinierId",
                table: "Dotations",
                newName: "IX_Dotations_MagasinierId");

            migrationBuilder.RenameIndex(
                name: "IX_Dotation_EmployeeId",
                table: "Dotations",
                newName: "IX_Dotations_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Dotation_CoordinatorId",
                table: "Dotations",
                newName: "IX_Dotations_CoordinatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DotationDetails",
                table: "DotationDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dotations",
                table: "Dotations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DotationDetails_Dotations_DotationId",
                table: "DotationDetails",
                column: "DotationId",
                principalTable: "Dotations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DotationDetails_PpeAttributeCategoryAttributeValues_ArticleId",
                table: "DotationDetails",
                column: "ArticleId",
                principalTable: "PpeAttributeCategoryAttributeValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dotations_Coordinateurs_CoordinatorId",
                table: "Dotations",
                column: "CoordinatorId",
                principalTable: "Coordinateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dotations_Employees_EmployeeId",
                table: "Dotations",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dotations_Magaziniers_MagasinierId",
                table: "Dotations",
                column: "MagasinierId",
                principalTable: "Magaziniers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dotations_Responsables_ResponsibleId",
                table: "Dotations",
                column: "ResponsibleId",
                principalTable: "Responsables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dotations_Transporteurs_TransporterId",
                table: "Dotations",
                column: "TransporterId",
                principalTable: "Transporteurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DotationDetails_Dotations_DotationId",
                table: "DotationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_DotationDetails_PpeAttributeCategoryAttributeValues_ArticleId",
                table: "DotationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Dotations_Coordinateurs_CoordinatorId",
                table: "Dotations");

            migrationBuilder.DropForeignKey(
                name: "FK_Dotations_Employees_EmployeeId",
                table: "Dotations");

            migrationBuilder.DropForeignKey(
                name: "FK_Dotations_Magaziniers_MagasinierId",
                table: "Dotations");

            migrationBuilder.DropForeignKey(
                name: "FK_Dotations_Responsables_ResponsibleId",
                table: "Dotations");

            migrationBuilder.DropForeignKey(
                name: "FK_Dotations_Transporteurs_TransporterId",
                table: "Dotations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dotations",
                table: "Dotations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DotationDetails",
                table: "DotationDetails");

            migrationBuilder.RenameTable(
                name: "Dotations",
                newName: "Dotation");

            migrationBuilder.RenameTable(
                name: "DotationDetails",
                newName: "DotationDetail");

            migrationBuilder.RenameIndex(
                name: "IX_Dotations_TransporterId",
                table: "Dotation",
                newName: "IX_Dotation_TransporterId");

            migrationBuilder.RenameIndex(
                name: "IX_Dotations_ResponsibleId",
                table: "Dotation",
                newName: "IX_Dotation_ResponsibleId");

            migrationBuilder.RenameIndex(
                name: "IX_Dotations_MagasinierId",
                table: "Dotation",
                newName: "IX_Dotation_MagasinierId");

            migrationBuilder.RenameIndex(
                name: "IX_Dotations_EmployeeId",
                table: "Dotation",
                newName: "IX_Dotation_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Dotations_CoordinatorId",
                table: "Dotation",
                newName: "IX_Dotation_CoordinatorId");

            migrationBuilder.RenameIndex(
                name: "IX_DotationDetails_DotationId",
                table: "DotationDetail",
                newName: "IX_DotationDetail_DotationId");

            migrationBuilder.RenameIndex(
                name: "IX_DotationDetails_ArticleId",
                table: "DotationDetail",
                newName: "IX_DotationDetail_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dotation",
                table: "Dotation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DotationDetail",
                table: "DotationDetail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dotation_Coordinateurs_CoordinatorId",
                table: "Dotation",
                column: "CoordinatorId",
                principalTable: "Coordinateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dotation_Employees_EmployeeId",
                table: "Dotation",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dotation_Magaziniers_MagasinierId",
                table: "Dotation",
                column: "MagasinierId",
                principalTable: "Magaziniers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dotation_Responsables_ResponsibleId",
                table: "Dotation",
                column: "ResponsibleId",
                principalTable: "Responsables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dotation_Transporteurs_TransporterId",
                table: "Dotation",
                column: "TransporterId",
                principalTable: "Transporteurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DotationDetail_Dotation_DotationId",
                table: "DotationDetail",
                column: "DotationId",
                principalTable: "Dotation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DotationDetail_PpeAttributeCategoryAttributeValues_ArticleId",
                table: "DotationDetail",
                column: "ArticleId",
                principalTable: "PpeAttributeCategoryAttributeValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
