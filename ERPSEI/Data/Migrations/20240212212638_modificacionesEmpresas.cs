using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class modificacionesEmpresas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActividadEconomica",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Deshabilitado",
                table: "Empresas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ObjetoSocial",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "URLWeb",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BancoEmpresa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Banco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Responsable = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Firmante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BancoEmpresa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BancoEmpresa_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 3, "Otro" });

            migrationBuilder.UpdateData(
                table: "TipoArchivoEmpresa",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "INE");

            migrationBuilder.InsertData(
                table: "TipoArchivoEmpresa",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "CSF" },
                    { 4, "Otro" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BancoEmpresa_EmpresaId",
                table: "BancoEmpresa",
                column: "EmpresaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BancoEmpresa");

            migrationBuilder.DeleteData(
                table: "Generos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TipoArchivoEmpresa",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TipoArchivoEmpresa",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "ActividadEconomica",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Deshabilitado",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "ObjetoSocial",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "URLWeb",
                table: "Empresas");

            migrationBuilder.UpdateData(
                table: "TipoArchivoEmpresa",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "CSF");
        }
    }
}
