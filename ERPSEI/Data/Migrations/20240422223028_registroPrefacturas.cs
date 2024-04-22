using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class registroPrefacturas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prefacturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
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
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prefacturas", x => x.Id);
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

            migrationBuilder.CreateIndex(
                name: "IX_Conceptos_ObjetoImpuestoId",
                table: "Conceptos",
                column: "ObjetoImpuestoId",
                unique: true);

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
                column: "UnidadMedidaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conceptos");

            migrationBuilder.DropTable(
                name: "Prefacturas");
        }
    }
}
