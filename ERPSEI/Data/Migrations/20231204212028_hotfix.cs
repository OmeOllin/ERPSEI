using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class hotfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileType");

            migrationBuilder.DropTable(
                name: "UserFiles");

            migrationBuilder.DropColumn(
                name: "FathersLastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MothersLastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SecondName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "EmpleadoId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPreregisterAuthorized",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PasswordResetNeeded",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
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
                    Id = table.Column<int>(type: "int", nullable: false),
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
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Oficinas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oficinas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Puestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puestos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoArchivo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoArchivo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subareas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subareas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subareas_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombrePreferido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoPaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoMaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CURP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RFC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NSS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneroId = table.Column<int>(type: "int", nullable: true),
                    SubareaId = table.Column<int>(type: "int", nullable: true),
                    OficinaId = table.Column<int>(type: "int", nullable: true),
                    PuestoId = table.Column<int>(type: "int", nullable: true),
                    AreaId = table.Column<int>(type: "int", nullable: true),
                    EstadoCivilId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    JefeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empleados_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Empleados_Empleados_JefeId",
                        column: x => x.JefeId,
                        principalTable: "Empleados",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Empleados_EstadosCiviles_EstadoCivilId",
                        column: x => x.EstadoCivilId,
                        principalTable: "EstadosCiviles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Empleados_Generos_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "Generos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Empleados_Oficinas_OficinaId",
                        column: x => x.OficinaId,
                        principalTable: "Oficinas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Empleados_Puestos_PuestoId",
                        column: x => x.PuestoId,
                        principalTable: "Puestos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Empleados_Subareas_SubareaId",
                        column: x => x.SubareaId,
                        principalTable: "Subareas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ArchivosEmpleado",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Archivo = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    TipoArchivoId = table.Column<int>(type: "int", nullable: true),
                    EmpleadoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivosEmpleado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchivosEmpleado_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ArchivosEmpleado_TipoArchivo_TipoArchivoId",
                        column: x => x.TipoArchivoId,
                        principalTable: "TipoArchivo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContactosEmergencia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpleadoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactosEmergencia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactosEmergencia_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Administración" },
                    { 2, "Auditoría" },
                    { 3, "Bancos" },
                    { 4, "Contabilidad" },
                    { 5, "Dirección General" },
                    { 6, "Expedientes" },
                    { 7, "Family Office" },
                    { 8, "Fiscal" },
                    { 9, "Impuestos" },
                    { 10, "Legal" },
                    { 11, "Nóminas" },
                    { 12, "Operaciones" },
                    { 13, "Recursos Humanos" },
                    { 14, "Tesorería" },
                    { 15, "Socio" }
                });

            migrationBuilder.InsertData(
                table: "EstadosCiviles",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Soltero" },
                    { 2, "Casado" }
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
                table: "Oficinas",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Austria 1" },
                    { 2, "Austria 6" },
                    { 3, "Big Ben" },
                    { 4, "Cancún" },
                    { 5, "Capri" },
                    { 6, "Centro Urbano" },
                    { 7, "Cóndor" },
                    { 8, "Izaguirre" },
                    { 9, "Lago de Guadalupe" },
                    { 10, "León" },
                    { 11, "Lomas Verdes" },
                    { 12, "Los Reyes La Paz" },
                    { 13, "Pafnuncio" },
                    { 14, "Pirules" },
                    { 15, "Polanco" },
                    { 16, "Santa Mónica" },
                    { 17, "Torre Esmeralda" }
                });

            migrationBuilder.InsertData(
                table: "Puestos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Analista" },
                    { 2, "Asistente" },
                    { 3, "Auditor" },
                    { 4, "Auxiliar" },
                    { 5, "Chofer" },
                    { 6, "Desarrollador" },
                    { 7, "Director" },
                    { 8, "Socio Director" },
                    { 9, "Encargado" },
                    { 10, "Gerente" },
                    { 11, "Mantenimiento y Limpieza" },
                    { 12, "Recepcionista" },
                    { 13, "Recepcionista Coordinadora" },
                    { 14, "Seguridad Privada" },
                    { 15, "Socio" },
                    { 16, "Subencargado" },
                    { 17, "Subgerente" },
                    { 18, "Supervisor" },
                    { 19, "Técnico" },
                    { 20, "Tesorero" }
                });

            migrationBuilder.InsertData(
                table: "TipoArchivo",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "Imagen de perfil" },
                    { 2, "Acta de nacimiento" },
                    { 3, "CURP" },
                    { 4, "CLABE" },
                    { 5, "Comprobante de domicilio" },
                    { 6, "Contactos de emergencia" },
                    { 7, "CSF" },
                    { 8, "INE" },
                    { 9, "RFC" },
                    { 10, "Comprobante de estudios" },
                    { 11, "NSS" },
                    { 12, "Otro" }
                });

            migrationBuilder.InsertData(
                table: "Subareas",
                columns: new[] { "Id", "AreaId", "Nombre" },
                values: new object[,]
                {
                    { 1, 1, "Sistemas" },
                    { 2, 4, "Interna" },
                    { 3, 4, "Externa" },
                    { 4, 4, "Impuestos" },
                    { 5, 5, "Control Vehicular" },
                    { 6, 12, "IMSS" },
                    { 7, 12, "Internas" },
                    { 8, 12, "Facturación" },
                    { 9, 12, "Nóminas" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmpleadoId",
                table: "AspNetUsers",
                column: "EmpleadoId",
                unique: true,
                filter: "[EmpleadoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ArchivosEmpleado_EmpleadoId",
                table: "ArchivosEmpleado",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_ArchivosEmpleado_TipoArchivoId",
                table: "ArchivosEmpleado",
                column: "TipoArchivoId");

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
                name: "IX_Empleados_JefeId",
                table: "Empleados",
                column: "JefeId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_OficinaId",
                table: "Empleados",
                column: "OficinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_PuestoId",
                table: "Empleados",
                column: "PuestoId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_SubareaId",
                table: "Empleados",
                column: "SubareaId");

            migrationBuilder.CreateIndex(
                name: "IX_Subareas_AreaId",
                table: "Subareas",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Empleados_EmpleadoId",
                table: "AspNetUsers",
                column: "EmpleadoId",
                principalTable: "Empleados",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Empleados_EmpleadoId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ArchivosEmpleado");

            migrationBuilder.DropTable(
                name: "ContactosEmergencia");

            migrationBuilder.DropTable(
                name: "TipoArchivo");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "EstadosCiviles");

            migrationBuilder.DropTable(
                name: "Generos");

            migrationBuilder.DropTable(
                name: "Oficinas");

            migrationBuilder.DropTable(
                name: "Puestos");

            migrationBuilder.DropTable(
                name: "Subareas");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmpleadoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmpleadoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsPreregisterAuthorized",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PasswordResetNeeded",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "FathersLastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MothersLastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "SecondName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FileType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FileTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFiles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FileType",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "Acta de nacimiento" },
                    { 2, "CURP" },
                    { 3, "CLABE" },
                    { 4, "Comprobante de domicilio" },
                    { 5, "Contactos de emergencia" },
                    { 6, "CSF" },
                    { 7, "INE" },
                    { 8, "RFC" },
                    { 9, "Comprobante de estudios" },
                    { 10, "NSS" }
                });
        }
    }
}
