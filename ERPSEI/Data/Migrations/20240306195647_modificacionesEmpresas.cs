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
            migrationBuilder.DropColumn(
                name: "Nivel",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Origen",
                table: "Empresas");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CorreoGeneral",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CorreoFiscal",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CorreoBancos",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Administrador",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Accionista",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CorreoFacturacion",
                table: "Empresas",
                type: "nvarchar(max)",
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
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicioAsimilados",
                table: "Empresas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicioFacturacion",
                table: "Empresas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicioOperacion",
                table: "Empresas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NivelId",
                table: "Empresas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObjetoSocial",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrigenId",
                table: "Empresas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PerfilId",
                table: "Empresas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActividadesEconomicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Perfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActividadesEconomicasEmpresa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: true),
                    ActividadEconomicaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActividadesEconomicasEmpresa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActividadesEconomicasEmpresa_ActividadesEconomicas_ActividadEconomicaId",
                        column: x => x.ActividadEconomicaId,
                        principalTable: "ActividadesEconomicas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActividadesEconomicasEmpresa_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductosServicios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IncluirIVATraslado = table.Column<bool>(type: "bit", nullable: false),
                    IncluirIEPSTraslado = table.Column<bool>(type: "bit", nullable: false),
                    PalabrasSimilares = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PerfilId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductosServicios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductosServicios_Perfiles_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfiles",
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
                    { 4, "ComprobanteDomicilio" },
                    { 5, "Otro" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_NivelId",
                table: "Empresas",
                column: "NivelId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_OrigenId",
                table: "Empresas",
                column: "OrigenId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_PerfilId",
                table: "Empresas",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_ActividadesEconomicasEmpresa_ActividadEconomicaId",
                table: "ActividadesEconomicasEmpresa",
                column: "ActividadEconomicaId");

            migrationBuilder.CreateIndex(
                name: "IX_ActividadesEconomicasEmpresa_EmpresaId",
                table: "ActividadesEconomicasEmpresa",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_BancosEmpresa_EmpresaId",
                table: "BancosEmpresa",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosServicios_PerfilId",
                table: "ProductosServicios",
                column: "PerfilId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_Perfiles_PerfilId",
                table: "Empresas",
                column: "PerfilId",
                principalTable: "Perfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_Niveles_NivelId",
                table: "Empresas");

            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_Origenes_OrigenId",
                table: "Empresas");

            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_Perfiles_PerfilId",
                table: "Empresas");

            migrationBuilder.DropTable(
                name: "ActividadesEconomicasEmpresa");

            migrationBuilder.DropTable(
                name: "BancosEmpresa");

            migrationBuilder.DropTable(
                name: "Niveles");

            migrationBuilder.DropTable(
                name: "Origenes");

            migrationBuilder.DropTable(
                name: "ProductosServicios");

            migrationBuilder.DropTable(
                name: "ActividadesEconomicas");

            migrationBuilder.DropTable(
                name: "Perfiles");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_NivelId",
                table: "Empresas");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_OrigenId",
                table: "Empresas");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_PerfilId",
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
                name: "CorreoFacturacion",
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
                name: "ObjetoSocial",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "OrigenId",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "PerfilId",
                table: "Empresas");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CorreoGeneral",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CorreoFiscal",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CorreoBancos",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Administrador",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Accionista",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nivel",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Origen",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "TipoArchivoEmpresa",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "CSF");
        }
    }
}
