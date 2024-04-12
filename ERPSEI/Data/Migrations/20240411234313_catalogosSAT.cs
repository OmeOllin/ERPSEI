using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class catalogosSAT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Deshabilitado",
                table: "ProductosServicios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Deshabilitado",
                table: "ActividadesEconomicas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Exportaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exportaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormasPago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bancarizado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroDeOperacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RFCEmisorCuentaOrdenante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CuentaOrdenante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatronCuentaOrdenante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RFCEmisorCuentaBeneficiario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CuentaBeneficiario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatronCuentaBeneficiario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoCadenaPago = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreBancoEmisorCuenta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormasPago", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Impuestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Retencion = table.Column<bool>(type: "bit", nullable: false),
                    Traslado = table.Column<bool>(type: "bit", nullable: false),
                    Local = table.Column<bool>(type: "bit", nullable: false),
                    Federal = table.Column<bool>(type: "bit", nullable: false),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impuestos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetodosPago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetodosPago", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Monedas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Decimales = table.Column<int>(type: "int", nullable: false),
                    PorcentajeVariacion = table.Column<double>(type: "float", nullable: false),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monedas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObjetosImpuesto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjetosImpuesto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Periodicidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periodicidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegimenesFiscales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AplicaPersonaFisica = table.Column<bool>(type: "bit", nullable: false),
                    AplicaPersonaMoral = table.Column<bool>(type: "bit", nullable: false),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegimenesFiscales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposComprobante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorMaximo = table.Column<decimal>(type: "decimal(24,6)", precision: 24, scale: 6, nullable: false),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposComprobante", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposFactor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposFactor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposRelacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposRelacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnidadesMedida",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nota = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Simbolo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadesMedida", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsosCFDI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AplicaPersonaFisica = table.Column<bool>(type: "bit", nullable: false),
                    AplicaPersonaMoral = table.Column<bool>(type: "bit", nullable: false),
                    RegimenFiscalReceptor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsosCFDI", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TasasOCuotas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Rango = table.Column<bool>(type: "bit", nullable: false),
                    Fijo = table.Column<bool>(type: "bit", nullable: false),
                    ValorMinimo = table.Column<double>(type: "float", nullable: false),
                    ValorMaximo = table.Column<double>(type: "float", nullable: false),
                    ImpuestoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FactorId = table.Column<int>(type: "int", nullable: false),
                    Traslado = table.Column<bool>(type: "bit", nullable: false),
                    Retencion = table.Column<bool>(type: "bit", nullable: false),
                    Deshabilitado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasasOCuotas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TasasOCuotas_Impuestos_ImpuestoId",
                        column: x => x.ImpuestoId,
                        principalTable: "Impuestos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TasasOCuotas_TiposFactor_FactorId",
                        column: x => x.FactorId,
                        principalTable: "TiposFactor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TasasOCuotas_FactorId",
                table: "TasasOCuotas",
                column: "FactorId");

            migrationBuilder.CreateIndex(
                name: "IX_TasasOCuotas_ImpuestoId",
                table: "TasasOCuotas",
                column: "ImpuestoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exportaciones");

            migrationBuilder.DropTable(
                name: "FormasPago");

            migrationBuilder.DropTable(
                name: "Meses");

            migrationBuilder.DropTable(
                name: "MetodosPago");

            migrationBuilder.DropTable(
                name: "Monedas");

            migrationBuilder.DropTable(
                name: "ObjetosImpuesto");

            migrationBuilder.DropTable(
                name: "Periodicidades");

            migrationBuilder.DropTable(
                name: "RegimenesFiscales");

            migrationBuilder.DropTable(
                name: "TasasOCuotas");

            migrationBuilder.DropTable(
                name: "TiposComprobante");

            migrationBuilder.DropTable(
                name: "TiposRelacion");

            migrationBuilder.DropTable(
                name: "UnidadesMedida");

            migrationBuilder.DropTable(
                name: "UsosCFDI");

            migrationBuilder.DropTable(
                name: "Impuestos");

            migrationBuilder.DropTable(
                name: "TiposFactor");

            migrationBuilder.DropColumn(
                name: "Deshabilitado",
                table: "ProductosServicios");

            migrationBuilder.DropColumn(
                name: "Deshabilitado",
                table: "ActividadesEconomicas");
        }
    }
}
