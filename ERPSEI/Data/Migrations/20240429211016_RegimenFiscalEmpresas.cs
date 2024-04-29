using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class RegimenFiscalEmpresas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegimenFiscalId",
                table: "Empresas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_RegimenFiscalId",
                table: "Empresas",
                column: "RegimenFiscalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_RegimenesFiscales_RegimenFiscalId",
                table: "Empresas",
                column: "RegimenFiscalId",
                principalTable: "RegimenesFiscales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_RegimenesFiscales_RegimenFiscalId",
                table: "Empresas");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_RegimenFiscalId",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "RegimenFiscalId",
                table: "Empresas");
        }
    }
}
