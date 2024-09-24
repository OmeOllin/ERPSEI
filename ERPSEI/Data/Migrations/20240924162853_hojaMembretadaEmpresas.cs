using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class hojaMembretadaEmpresas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TipoArchivoEmpresa",
                columns: new[] { "Id", "Description" },
                values: new object[] { 9, "HojaMembretada" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TipoArchivoEmpresa",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
