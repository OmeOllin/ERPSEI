using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class entidadesCfdi40 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComprobantesConceptosACuentaTerceros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RfcACuentaTerceros = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreACuentaTerceros = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegimenFiscalACuentaTerceros = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DomicilioFiscalACuentaTerceros = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesConceptosACuentaTerceros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesConceptosImpuestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesConceptosImpuestos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesEmisores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rfc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegimenFiscal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacAtrAdquirente = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesEmisores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesImpuestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalImpuestosRetenidos = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalImpuestosRetenidosSpecified = table.Column<bool>(type: "bit", nullable: false),
                    TotalImpuestosTrasladados = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalImpuestosTrasladadosSpecified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesImpuestos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesInformacionesGlobales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Periodicidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Meses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Año = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesInformacionesGlobales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesReceptores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rfc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DomicilioFiscalReceptor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResidenciaFiscal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResidenciaFiscalSpecified = table.Column<bool>(type: "bit", nullable: false),
                    NumRegIdTrib = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegimenFiscalReceptor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsoCFDI = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesReceptores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesConceptosImpuestosRetenciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Base = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Impuesto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoFactor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TasaOCuota = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Importe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ComprobanteConceptoImpuestosId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesConceptosImpuestosRetenciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobantesConceptosImpuestosRetenciones_ComprobantesConceptosImpuestos_ComprobanteConceptoImpuestosId",
                        column: x => x.ComprobanteConceptoImpuestosId,
                        principalTable: "ComprobantesConceptosImpuestos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesConceptosImpuestosTraslados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Base = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Impuesto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoFactor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TasaOCuota = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TasaOCuotaSpecified = table.Column<bool>(type: "bit", nullable: false),
                    Importe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ImporteSpecified = table.Column<bool>(type: "bit", nullable: false),
                    ComprobanteConceptoImpuestosId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesConceptosImpuestosTraslados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobantesConceptosImpuestosTraslados_ComprobantesConceptosImpuestos_ComprobanteConceptoImpuestosId",
                        column: x => x.ComprobanteConceptoImpuestosId,
                        principalTable: "ComprobantesConceptosImpuestos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesImpuestosRetenciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Impuesto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Importe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ComprobanteImpuestosId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesImpuestosRetenciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobantesImpuestosRetenciones_ComprobantesImpuestos_ComprobanteImpuestosId",
                        column: x => x.ComprobanteImpuestosId,
                        principalTable: "ComprobantesImpuestos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesImpuestosTraslados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Base = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Impuesto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoFactor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TasaOCuota = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TasaOCuotaSpecified = table.Column<bool>(type: "bit", nullable: false),
                    Importe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ImporteSpecified = table.Column<bool>(type: "bit", nullable: false),
                    ComprobanteImpuestosId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesImpuestosTraslados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobantesImpuestosTraslados_ComprobantesImpuestos_ComprobanteImpuestosId",
                        column: x => x.ComprobanteImpuestosId,
                        principalTable: "ComprobantesImpuestos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comprobantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InformacionGlobalId = table.Column<int>(type: "int", nullable: true),
                    EmisorId = table.Column<int>(type: "int", nullable: true),
                    ReceptorId = table.Column<int>(type: "int", nullable: true),
                    ImpuestosId = table.Column<int>(type: "int", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Serie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Folio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sello = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormaPago = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormaPagoSpecified = table.Column<bool>(type: "bit", nullable: false),
                    NoCertificado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Certificado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CondicionesDePago = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DescuentoSpecified = table.Column<bool>(type: "bit", nullable: false),
                    Moneda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoCambio = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TipoCambioSpecified = table.Column<bool>(type: "bit", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TipoDeComprobante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Exportacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetodoPago = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetodoPagoSpecified = table.Column<bool>(type: "bit", nullable: false),
                    LugarExpedicion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Confirmacion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comprobantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comprobantes_ComprobantesEmisores_EmisorId",
                        column: x => x.EmisorId,
                        principalTable: "ComprobantesEmisores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comprobantes_ComprobantesImpuestos_ImpuestosId",
                        column: x => x.ImpuestosId,
                        principalTable: "ComprobantesImpuestos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comprobantes_ComprobantesInformacionesGlobales_InformacionGlobalId",
                        column: x => x.InformacionGlobalId,
                        principalTable: "ComprobantesInformacionesGlobales",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comprobantes_ComprobantesReceptores_ReceptorId",
                        column: x => x.ReceptorId,
                        principalTable: "ComprobantesReceptores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesCfdisRelacionados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoRelacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComprobanteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesCfdisRelacionados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobantesCfdisRelacionados_Comprobantes_ComprobanteId",
                        column: x => x.ComprobanteId,
                        principalTable: "Comprobantes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesConceptos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImpuestosId = table.Column<int>(type: "int", nullable: true),
                    ACuentaTercerosId = table.Column<int>(type: "int", nullable: true),
                    ClaveProdServ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoIdentificacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ClaveUnidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Importe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DescuentoSpecified = table.Column<bool>(type: "bit", nullable: false),
                    ObjetoImp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComprobanteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesConceptos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobantesConceptos_ComprobantesConceptosACuentaTerceros_ACuentaTercerosId",
                        column: x => x.ACuentaTercerosId,
                        principalTable: "ComprobantesConceptosACuentaTerceros",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComprobantesConceptos_ComprobantesConceptosImpuestos_ImpuestosId",
                        column: x => x.ImpuestosId,
                        principalTable: "ComprobantesConceptosImpuestos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComprobantesConceptos_Comprobantes_ComprobanteId",
                        column: x => x.ComprobanteId,
                        principalTable: "Comprobantes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComprobanteCfdiRelacionadosCfdiRelacionado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UUID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComprobanteCfdiRelacionadosId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobanteCfdiRelacionadosCfdiRelacionado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobanteCfdiRelacionadosCfdiRelacionado_ComprobantesCfdisRelacionados_ComprobanteCfdiRelacionadosId",
                        column: x => x.ComprobanteCfdiRelacionadosId,
                        principalTable: "ComprobantesCfdisRelacionados",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesConceptosCuentasPrediales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComprobanteConceptoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesConceptosCuentasPrediales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobantesConceptosCuentasPrediales_ComprobantesConceptos_ComprobanteConceptoId",
                        column: x => x.ComprobanteConceptoId,
                        principalTable: "ComprobantesConceptos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesConceptosInformacionesAduaneras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroPedimento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComprobanteConceptoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesConceptosInformacionesAduaneras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobantesConceptosInformacionesAduaneras_ComprobantesConceptos_ComprobanteConceptoId",
                        column: x => x.ComprobanteConceptoId,
                        principalTable: "ComprobantesConceptos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesConceptosPartes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaveProdServ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoIdentificacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Unidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ValorUnitarioSpecified = table.Column<bool>(type: "bit", nullable: false),
                    Importe = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ImporteSpecified = table.Column<bool>(type: "bit", nullable: false),
                    ComprobanteConceptoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesConceptosPartes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobantesConceptosPartes_ComprobantesConceptos_ComprobanteConceptoId",
                        column: x => x.ComprobanteConceptoId,
                        principalTable: "ComprobantesConceptos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComprobantesConceptosPartesInformacionesAduaneras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroPedimento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComprobanteConceptoParteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobantesConceptosPartesInformacionesAduaneras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprobantesConceptosPartesInformacionesAduaneras_ComprobantesConceptosPartes_ComprobanteConceptoParteId",
                        column: x => x.ComprobanteConceptoParteId,
                        principalTable: "ComprobantesConceptosPartes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteCfdiRelacionadosCfdiRelacionado_ComprobanteCfdiRelacionadosId",
                table: "ComprobanteCfdiRelacionadosCfdiRelacionado",
                column: "ComprobanteCfdiRelacionadosId");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_EmisorId",
                table: "Comprobantes",
                column: "EmisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_ImpuestosId",
                table: "Comprobantes",
                column: "ImpuestosId");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_InformacionGlobalId",
                table: "Comprobantes",
                column: "InformacionGlobalId");

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_ReceptorId",
                table: "Comprobantes",
                column: "ReceptorId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobantesCfdisRelacionados_ComprobanteId",
                table: "ComprobantesCfdisRelacionados",
                column: "ComprobanteId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobantesConceptos_ACuentaTercerosId",
                table: "ComprobantesConceptos",
                column: "ACuentaTercerosId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobantesConceptos_ComprobanteId",
                table: "ComprobantesConceptos",
                column: "ComprobanteId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobantesConceptos_ImpuestosId",
                table: "ComprobantesConceptos",
                column: "ImpuestosId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobantesConceptosCuentasPrediales_ComprobanteConceptoId",
                table: "ComprobantesConceptosCuentasPrediales",
                column: "ComprobanteConceptoId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobantesConceptosImpuestosRetenciones_ComprobanteConceptoImpuestosId",
                table: "ComprobantesConceptosImpuestosRetenciones",
                column: "ComprobanteConceptoImpuestosId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobantesConceptosImpuestosTraslados_ComprobanteConceptoImpuestosId",
                table: "ComprobantesConceptosImpuestosTraslados",
                column: "ComprobanteConceptoImpuestosId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobantesConceptosInformacionesAduaneras_ComprobanteConceptoId",
                table: "ComprobantesConceptosInformacionesAduaneras",
                column: "ComprobanteConceptoId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobantesConceptosPartes_ComprobanteConceptoId",
                table: "ComprobantesConceptosPartes",
                column: "ComprobanteConceptoId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobantesConceptosPartesInformacionesAduaneras_ComprobanteConceptoParteId",
                table: "ComprobantesConceptosPartesInformacionesAduaneras",
                column: "ComprobanteConceptoParteId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobantesImpuestosRetenciones_ComprobanteImpuestosId",
                table: "ComprobantesImpuestosRetenciones",
                column: "ComprobanteImpuestosId");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobantesImpuestosTraslados_ComprobanteImpuestosId",
                table: "ComprobantesImpuestosTraslados",
                column: "ComprobanteImpuestosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComprobanteCfdiRelacionadosCfdiRelacionado");

            migrationBuilder.DropTable(
                name: "ComprobantesConceptosCuentasPrediales");

            migrationBuilder.DropTable(
                name: "ComprobantesConceptosImpuestosRetenciones");

            migrationBuilder.DropTable(
                name: "ComprobantesConceptosImpuestosTraslados");

            migrationBuilder.DropTable(
                name: "ComprobantesConceptosInformacionesAduaneras");

            migrationBuilder.DropTable(
                name: "ComprobantesConceptosPartesInformacionesAduaneras");

            migrationBuilder.DropTable(
                name: "ComprobantesImpuestosRetenciones");

            migrationBuilder.DropTable(
                name: "ComprobantesImpuestosTraslados");

            migrationBuilder.DropTable(
                name: "ComprobantesCfdisRelacionados");

            migrationBuilder.DropTable(
                name: "ComprobantesConceptosPartes");

            migrationBuilder.DropTable(
                name: "ComprobantesConceptos");

            migrationBuilder.DropTable(
                name: "ComprobantesConceptosACuentaTerceros");

            migrationBuilder.DropTable(
                name: "ComprobantesConceptosImpuestos");

            migrationBuilder.DropTable(
                name: "Comprobantes");

            migrationBuilder.DropTable(
                name: "ComprobantesEmisores");

            migrationBuilder.DropTable(
                name: "ComprobantesImpuestos");

            migrationBuilder.DropTable(
                name: "ComprobantesInformacionesGlobales");

            migrationBuilder.DropTable(
                name: "ComprobantesReceptores");
        }
    }
}
