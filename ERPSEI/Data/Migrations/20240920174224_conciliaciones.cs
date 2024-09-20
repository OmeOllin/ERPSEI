using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class conciliaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bancos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deshabilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bancos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    RegimenFiscalId = table.Column<int>(type: "int", nullable: false),
                    UsoCFDIId = table.Column<int>(type: "int", nullable: false),
                    RazonSocial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RFC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DomicilioFiscal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResidenciaFiscal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deshabilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clientes_RegimenesFiscales_RegimenFiscalId",
                        column: x => x.RegimenFiscalId,
                        principalTable: "RegimenesFiscales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clientes_UsosCFDI_UsoCFDIId",
                        column: x => x.UsoCFDIId,
                        principalTable: "UsosCFDI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conciliaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(24,6)", precision: 24, scale: 6, nullable: true),
                    BancoId = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioCreadorId = table.Column<int>(type: "int", nullable: false),
                    AppUserCId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UsuarioModificadorId = table.Column<int>(type: "int", nullable: false),
                    AppUserMId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Deshabilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conciliaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conciliaciones_AspNetUsers_AppUserCId",
                        column: x => x.AppUserCId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Conciliaciones_AspNetUsers_AppUserMId",
                        column: x => x.AppUserMId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Conciliaciones_Bancos_BancoId",
                        column: x => x.BancoId,
                        principalTable: "Bancos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Conciliaciones_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Conciliaciones_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ConciliacionesDetalles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ConciliacionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConciliacionesDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConciliacionesDetalles_Conciliaciones_ConciliacionId",
                        column: x => x.ConciliacionId,
                        principalTable: "Conciliaciones",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MovimientosBancarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Importe = table.Column<decimal>(type: "decimal(24,6)", precision: 24, scale: 6, nullable: true),
                    Conciliado = table.Column<bool>(type: "bit", nullable: false),
                    ConciliacionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosBancarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimientosBancarios_Conciliaciones_ConciliacionId",
                        column: x => x.ConciliacionId,
                        principalTable: "Conciliaciones",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ConciliacionesDetallesComprobantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ConciliacionDetalleId = table.Column<int>(type: "int", nullable: true),
                    ComprobanteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConciliacionesDetallesComprobantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConciliacionesDetallesComprobantes_Comprobantes_ComprobanteId",
                        column: x => x.ComprobanteId,
                        principalTable: "Comprobantes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConciliacionesDetallesComprobantes_ConciliacionesDetalles_ConciliacionDetalleId",
                        column: x => x.ConciliacionDetalleId,
                        principalTable: "ConciliacionesDetalles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ConciliacionesDetallesMovimientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ConciliacionDetalleId = table.Column<int>(type: "int", nullable: true),
                    MovimientoBancarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConciliacionesDetallesMovimientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConciliacionesDetallesMovimientos_ConciliacionesDetalles_ConciliacionDetalleId",
                        column: x => x.ConciliacionDetalleId,
                        principalTable: "ConciliacionesDetalles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConciliacionesDetallesMovimientos_MovimientosBancarios_MovimientoBancarioId",
                        column: x => x.MovimientoBancarioId,
                        principalTable: "MovimientosBancarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_RegimenFiscalId",
                table: "Clientes",
                column: "RegimenFiscalId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_UsoCFDIId",
                table: "Clientes",
                column: "UsoCFDIId");

            migrationBuilder.CreateIndex(
                name: "IX_Conciliaciones_AppUserCId",
                table: "Conciliaciones",
                column: "AppUserCId");

            migrationBuilder.CreateIndex(
                name: "IX_Conciliaciones_AppUserMId",
                table: "Conciliaciones",
                column: "AppUserMId");

            migrationBuilder.CreateIndex(
                name: "IX_Conciliaciones_BancoId",
                table: "Conciliaciones",
                column: "BancoId");

            migrationBuilder.CreateIndex(
                name: "IX_Conciliaciones_ClienteId",
                table: "Conciliaciones",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Conciliaciones_EmpresaId",
                table: "Conciliaciones",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConciliacionesDetalles_ConciliacionId",
                table: "ConciliacionesDetalles",
                column: "ConciliacionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConciliacionesDetallesComprobantes_ComprobanteId",
                table: "ConciliacionesDetallesComprobantes",
                column: "ComprobanteId",
                unique: true,
                filter: "[ComprobanteId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ConciliacionesDetallesComprobantes_ConciliacionDetalleId",
                table: "ConciliacionesDetallesComprobantes",
                column: "ConciliacionDetalleId");

            migrationBuilder.CreateIndex(
                name: "IX_ConciliacionesDetallesMovimientos_ConciliacionDetalleId",
                table: "ConciliacionesDetallesMovimientos",
                column: "ConciliacionDetalleId");

            migrationBuilder.CreateIndex(
                name: "IX_ConciliacionesDetallesMovimientos_MovimientoBancarioId",
                table: "ConciliacionesDetallesMovimientos",
                column: "MovimientoBancarioId",
                unique: true,
                filter: "[MovimientoBancarioId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosBancarios_ConciliacionId",
                table: "MovimientosBancarios",
                column: "ConciliacionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConciliacionesDetallesComprobantes");

            migrationBuilder.DropTable(
                name: "ConciliacionesDetallesMovimientos");

            migrationBuilder.DropTable(
                name: "ConciliacionesDetalles");

            migrationBuilder.DropTable(
                name: "MovimientosBancarios");

            migrationBuilder.DropTable(
                name: "Conciliaciones");

            migrationBuilder.DropTable(
                name: "Bancos");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
