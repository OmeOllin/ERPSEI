using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class registroPrefacturas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstatusPrefactura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstatusPrefactura", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prefacturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EmisorId = table.Column<int>(type: "int", nullable: false),
                    ReceptorId = table.Column<int>(type: "int", nullable: false),
                    Serie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Folio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoComprobanteId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonedaId = table.Column<int>(type: "int", nullable: false),
                    TipoCambio = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    FormaPagoId = table.Column<int>(type: "int", nullable: false),
                    MetodoPagoId = table.Column<int>(type: "int", nullable: false),
                    UsoCFDIId = table.Column<int>(type: "int", nullable: false),
                    ExportacionId = table.Column<int>(type: "int", nullable: true),
                    NumeroOperacion = table.Column<int>(type: "int", nullable: true),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false),
                    EstatusId = table.Column<int>(type: "int", nullable: true),
                    UsuarioCreadorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UsuarioAutorizadorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UsuarioFinalizadorId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prefacturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prefacturas_AspNetUsers_UsuarioAutorizadorId",
                        column: x => x.UsuarioAutorizadorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prefacturas_AspNetUsers_UsuarioCreadorId",
                        column: x => x.UsuarioCreadorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prefacturas_AspNetUsers_UsuarioFinalizadorId",
                        column: x => x.UsuarioFinalizadorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prefacturas_Empresas_EmisorId",
                        column: x => x.EmisorId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prefacturas_Empresas_ReceptorId",
                        column: x => x.ReceptorId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prefacturas_EstatusPrefactura_EstatusId",
                        column: x => x.EstatusId,
                        principalTable: "EstatusPrefactura",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prefacturas_Exportaciones_ExportacionId",
                        column: x => x.ExportacionId,
                        principalTable: "Exportaciones",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prefacturas_FormasPago_FormaPagoId",
                        column: x => x.FormaPagoId,
                        principalTable: "FormasPago",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prefacturas_MetodosPago_MetodoPagoId",
                        column: x => x.MetodoPagoId,
                        principalTable: "MetodosPago",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prefacturas_Monedas_MonedaId",
                        column: x => x.MonedaId,
                        principalTable: "Monedas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prefacturas_TiposComprobante_TipoComprobanteId",
                        column: x => x.TipoComprobanteId,
                        principalTable: "TiposComprobante",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prefacturas_UsosCFDI_UsoCFDIId",
                        column: x => x.UsoCFDIId,
                        principalTable: "UsosCFDI",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Conceptos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ProductoServicioId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    UnidadMedidaId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObjetoImpuestoId = table.Column<int>(type: "int", nullable: false),
                    TasaTraslado = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TasaRetencion = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PrefacturaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conceptos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conceptos_ObjetosImpuesto_ObjetoImpuestoId",
                        column: x => x.ObjetoImpuestoId,
                        principalTable: "ObjetosImpuesto",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Conceptos_Prefacturas_PrefacturaId",
                        column: x => x.PrefacturaId,
                        principalTable: "Prefacturas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Conceptos_ProductosServicios_ProductoServicioId",
                        column: x => x.ProductoServicioId,
                        principalTable: "ProductosServicios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conceptos_UnidadesMedida_UnidadMedidaId",
                        column: x => x.UnidadMedidaId,
                        principalTable: "UnidadesMedida",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "EstatusPrefactura",
                columns: new[] { "Id", "Descripcion", "Deshabilitado" },
                values: new object[,]
                {
                    { 1, "Solicitada", 0 },
                    { 2, "Autorizada", 0 },
                    { 3, "Finalizada", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conceptos_ObjetoImpuestoId",
                table: "Conceptos",
                column: "ObjetoImpuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_Conceptos_PrefacturaId",
                table: "Conceptos",
                column: "PrefacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_Conceptos_ProductoServicioId",
                table: "Conceptos",
                column: "ProductoServicioId");

            migrationBuilder.CreateIndex(
                name: "IX_Conceptos_UnidadMedidaId",
                table: "Conceptos",
                column: "UnidadMedidaId");

            migrationBuilder.CreateIndex(
                name: "IX_Prefacturas_EmisorId",
                table: "Prefacturas",
                column: "EmisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Prefacturas_EstatusId",
                table: "Prefacturas",
                column: "EstatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Prefacturas_ExportacionId",
                table: "Prefacturas",
                column: "ExportacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Prefacturas_FormaPagoId",
                table: "Prefacturas",
                column: "FormaPagoId");

            migrationBuilder.CreateIndex(
                name: "IX_Prefacturas_MetodoPagoId",
                table: "Prefacturas",
                column: "MetodoPagoId");

            migrationBuilder.CreateIndex(
                name: "IX_Prefacturas_MonedaId",
                table: "Prefacturas",
                column: "MonedaId");

            migrationBuilder.CreateIndex(
                name: "IX_Prefacturas_ReceptorId",
                table: "Prefacturas",
                column: "ReceptorId");

            migrationBuilder.CreateIndex(
                name: "IX_Prefacturas_TipoComprobanteId",
                table: "Prefacturas",
                column: "TipoComprobanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Prefacturas_UsoCFDIId",
                table: "Prefacturas",
                column: "UsoCFDIId");

            migrationBuilder.CreateIndex(
                name: "IX_Prefacturas_UsuarioAutorizadorId",
                table: "Prefacturas",
                column: "UsuarioAutorizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Prefacturas_UsuarioCreadorId",
                table: "Prefacturas",
                column: "UsuarioCreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Prefacturas_UsuarioFinalizadorId",
                table: "Prefacturas",
                column: "UsuarioFinalizadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conceptos");

            migrationBuilder.DropTable(
                name: "Prefacturas");

            migrationBuilder.DropTable(
                name: "EstatusPrefactura");
        }
    }
}
