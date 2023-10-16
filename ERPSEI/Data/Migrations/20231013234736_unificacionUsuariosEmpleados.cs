using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class unificacionUsuariosEmpleados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserFiles");

            migrationBuilder.AddColumn<int>(
                name: "EmpleadoId",
                table: "UserFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmpleadoId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadosCiviles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosCiviles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Generos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Puestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puestos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApellidoPaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoMaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CURP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RFC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NSS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelefonoPersonal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneroId = table.Column<int>(type: "int", nullable: false),
                    PuestoId = table.Column<int>(type: "int", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false),
                    EstadoCivilId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empleados_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Empleados_EstadosCiviles_EstadoCivilId",
                        column: x => x.EstadoCivilId,
                        principalTable: "EstadosCiviles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Empleados_Generos_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "Generos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Empleados_Puestos_PuestoId",
                        column: x => x.PuestoId,
                        principalTable: "Puestos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactosEmergencia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactosEmergencia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactosEmergencia_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Asistente Dirección" },
                    { 2, "Auditoría" },
                    { 3, "Bancos" },
                    { 4, "Contabilidad" },
                    { 5, "Director Administrativo" },
                    { 6, "Entregables" },
                    { 7, "Facturación" },
                    { 8, "Fiscal" },
                    { 9, "Impuestos" },
                    { 10, "IMSS" },
                    { 11, "Legal" },
                    { 12, "Nómina" },
                    { 13, "Nómina Externo" },
                    { 14, "Operaciones" },
                    { 15, "Operaciones Internas" },
                    { 16, "Operaciones Nómina" },
                    { 17, "Recursos Humanos" },
                    { 18, "Sistemas" },
                    { 19, "Soporte Dirección" },
                    { 20, "Tesorería" }
                });

            migrationBuilder.InsertData(
                table: "EstadosCiviles",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Soltero(a)" },
                    { 2, "Casado(a)" }
                });

            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Masculino" },
                    { 2, "Femenino" }
                });

            migrationBuilder.InsertData(
                table: "Puestos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Analista de IMSS" },
                    { 2, "Analista de Nómina" },
                    { 3, "Asistente AC" },
                    { 4, "Asistente de Dirección" },
                    { 5, "Asistente de Dirección / Julio" },
                    { 6, "Asistente de Dirección / Paco" },
                    { 7, "Asistente de Dirección / Alex" },
                    { 8, "Auditoría" },
                    { 9, "Auxiliar de Tesorería" },
                    { 10, "Auxiliar Administrativo" },
                    { 11, "Auxiliar Contable" },
                    { 12, "Auxiliar Contable Austria 6" },
                    { 13, "Auxiliar Contable Julio" },
                    { 14, "Auxiliar Contable Nómina Julio" },
                    { 15, "Auxiliar de Área Legal" },
                    { 16, "Auxiliar de Auditoría" },
                    { 17, "Auxiliar de Bancos" },
                    { 18, "Auxiliar de Entregables" },
                    { 19, "Auxiliar de Impuestos" },
                    { 20, "Auxiliar de Nómina" },
                    { 21, "Auxiliar de Nómina Cancún" },
                    { 22, "Auxiliar de Recursos Humanos" },
                    { 23, "Auxiliar de Sistemas" },
                    { 24, "Auxiliar de Tesorería" },
                    { 25, "Auxiliar IMSS" },
                    { 26, "Chofer" },
                    { 27, "Director Administrativo" },
                    { 28, "Director General" },
                    { 29, "Encargado de Impuestos" },
                    { 30, "Encargado de Contabilidad" },
                    { 31, "Encargado de IMSS" },
                    { 32, "Encargado de Nómina" },
                    { 33, "Facturación" },
                    { 34, "Gerente de Área Legal" },
                    { 35, "Gerente de Auditoría" },
                    { 36, "Gerente de Bancos" },
                    { 37, "Gerente de Contabilidad" },
                    { 38, "Gerente de Entregables" },
                    { 39, "Gerente de Impuestos" },
                    { 40, "Gerente de Nómina" },
                    { 41, "Gerente de Operaciones" },
                    { 42, "Gerente de Operaciones Internas" },
                    { 43, "Gerente de Recursos Humanos" },
                    { 44, "Inplant Nóminas/Asimilados" },
                    { 45, "Mantenimiento y Limpieza" },
                    { 46, "Nóminas Brame/Asimilados" },
                    { 47, "Pasante del Área Legal Clarisa" },
                    { 48, "Recepción Entregables Pafnuncio piso 5" },
                    { 49, "Recepción Lago de Guadalupe" },
                    { 50, "Recepción Los Reyes la Paz" },
                    { 51, "Recepcionista Austria 6" },
                    { 52, "Recepcionista Big 407" },
                    { 53, "Recepcionista Big 510" },
                    { 54, "Recepcionista Izaguirre" },
                    { 55, "Recepcionista Pirules" },
                    { 56, "Recepcionista Polanco" },
                    { 57, "Recepcionista Sta Mónica" },
                    { 58, "Recepcionista Torres Esmeralda" },
                    { 59, "Recepcionista y Apoyo Facturación Big 303" },
                    { 60, "Recepcionista Condor" },
                    { 61, "Senior de Soporte - Desarrollador de Software Izcalli 1" },
                    { 62, "Sistemas" },
                    { 63, "Subencargado de Nómina" },
                    { 64, "Supervisor Contable" },
                    { 65, "Supervisor de Contabilidad" },
                    { 66, "Supervisor Entregables" },
                    { 67, "Supervisor / Encargado de Nómina" },
                    { 68, "Tesorero" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFiles_EmpleadoId",
                table: "UserFiles",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFiles_FileTypeId",
                table: "UserFiles",
                column: "FileTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmpleadoId",
                table: "AspNetUsers",
                column: "EmpleadoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactosEmergencia_EmpleadoId",
                table: "ContactosEmergencia",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_AreaId",
                table: "Empleados",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_EstadoCivilId",
                table: "Empleados",
                column: "EstadoCivilId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_GeneroId",
                table: "Empleados",
                column: "GeneroId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_PuestoId",
                table: "Empleados",
                column: "PuestoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Empleados_EmpleadoId",
                table: "AspNetUsers",
                column: "EmpleadoId",
                principalTable: "Empleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFiles_Empleados_EmpleadoId",
                table: "UserFiles",
                column: "EmpleadoId",
                principalTable: "Empleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFiles_FileType_FileTypeId",
                table: "UserFiles",
                column: "FileTypeId",
                principalTable: "FileType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Empleados_EmpleadoId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFiles_Empleados_EmpleadoId",
                table: "UserFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFiles_FileType_FileTypeId",
                table: "UserFiles");

            migrationBuilder.DropTable(
                name: "ContactosEmergencia");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "EstadosCiviles");

            migrationBuilder.DropTable(
                name: "Generos");

            migrationBuilder.DropTable(
                name: "Puestos");

            migrationBuilder.DropIndex(
                name: "IX_UserFiles_EmpleadoId",
                table: "UserFiles");

            migrationBuilder.DropIndex(
                name: "IX_UserFiles_FileTypeId",
                table: "UserFiles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmpleadoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmpleadoId",
                table: "UserFiles");

            migrationBuilder.DropColumn(
                name: "EmpleadoId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
