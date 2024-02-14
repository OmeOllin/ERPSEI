using System;
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
            migrationBuilder.RenameColumn(
                name: "Origen",
                table: "Empresas",
                newName: "ObjetoSocial");

            migrationBuilder.RenameColumn(
                name: "Nivel",
                table: "Empresas",
                newName: "CorreoFacturacion");

            migrationBuilder.AddColumn<int>(
                name: "ActividadEconomicaId",
                table: "Empresas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Deshabilitado",
                table: "Empresas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaConstitucion",
                table: "Empresas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicioAsimilados",
                table: "Empresas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicioFacturacion",
                table: "Empresas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicioOperacion",
                table: "Empresas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "NivelId",
                table: "Empresas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrigenId",
                table: "Empresas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActividadesEconomicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActividadesEconomicas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BancosEmpresa",
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
                    table.PrimaryKey("PK_BancosEmpresa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BancosEmpresa_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Niveles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Niveles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Origenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Origenes", x => x.Id);
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
                    { 4, "ComprobanteDomicilio" },
                    { 5, "Otro" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_ActividadEconomicaId",
                table: "Empresas",
                column: "ActividadEconomicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_NivelId",
                table: "Empresas",
                column: "NivelId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_OrigenId",
                table: "Empresas",
                column: "OrigenId");

            migrationBuilder.CreateIndex(
                name: "IX_BancosEmpresa_EmpresaId",
                table: "BancosEmpresa",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_ActividadesEconomicas_ActividadEconomicaId",
                table: "Empresas",
                column: "ActividadEconomicaId",
                principalTable: "ActividadesEconomicas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_Niveles_NivelId",
                table: "Empresas",
                column: "NivelId",
                principalTable: "Niveles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_Origenes_OrigenId",
                table: "Empresas",
                column: "OrigenId",
                principalTable: "Origenes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_ActividadesEconomicas_ActividadEconomicaId",
                table: "Empresas");

            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_Niveles_NivelId",
                table: "Empresas");

            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_Origenes_OrigenId",
                table: "Empresas");

            migrationBuilder.DropTable(
                name: "ActividadesEconomicas");

            migrationBuilder.DropTable(
                name: "BancosEmpresa");

            migrationBuilder.DropTable(
                name: "Niveles");

            migrationBuilder.DropTable(
                name: "Origenes");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_ActividadEconomicaId",
                table: "Empresas");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_NivelId",
                table: "Empresas");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_OrigenId",
                table: "Empresas");

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

            migrationBuilder.DeleteData(
                table: "TipoArchivoEmpresa",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "ActividadEconomicaId",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Deshabilitado",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "FechaConstitucion",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "FechaInicioAsimilados",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "FechaInicioFacturacion",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "FechaInicioOperacion",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "NivelId",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "OrigenId",
                table: "Empresas");

            migrationBuilder.RenameColumn(
                name: "ObjetoSocial",
                table: "Empresas",
                newName: "Origen");

            migrationBuilder.RenameColumn(
                name: "CorreoFacturacion",
                table: "Empresas",
                newName: "Nivel");

            migrationBuilder.UpdateData(
                table: "TipoArchivoEmpresa",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "CSF");
        }
    }
}
