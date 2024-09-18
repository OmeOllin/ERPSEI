using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class cambiosModulos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Conciliado",
                table: "Comprobantes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UUID",
                table: "Comprobantes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Modulos",
                columns: new[] { "Id", "Categoria", "Deshabilitado", "Nombre", "NombreNormalizado" },
                values: new object[,]
                {
                    { 18, "erp", 0, "Activos Fijos", "activosfijos" },
                    { 19, "erp", 0, "Conciliaciones", "conciliaciones" },
                    { 20, "erp", 0, "Administrador de Comprobantes", "administradordecomprobantes" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Modulos",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Modulos",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Modulos",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DropColumn(
                name: "Conciliado",
                table: "Comprobantes");

            migrationBuilder.DropColumn(
                name: "UUID",
                table: "Comprobantes");
        }
    }
}
