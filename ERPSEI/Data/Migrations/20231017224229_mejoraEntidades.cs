using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class mejoraEntidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactosEmergencia_Empleados_EmpleadoId",
                table: "ContactosEmergencia");

            migrationBuilder.DropTable(
                name: "UserFiles");

            migrationBuilder.DropTable(
                name: "FileType");

            migrationBuilder.DropColumn(
                name: "CURP",
                table: "Empleados");

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

            migrationBuilder.RenameColumn(
                name: "TelefonoPersonal",
                table: "Empleados",
                newName: "Telefono");

            migrationBuilder.RenameColumn(
                name: "RFC",
                table: "Empleados",
                newName: "SegundoNombre");

            migrationBuilder.RenameColumn(
                name: "NSS",
                table: "Empleados",
                newName: "PrimerNombre");

            migrationBuilder.AddColumn<int>(
                name: "ArchivoEmpleadoId",
                table: "Empleados",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "Empleados",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ArchivosEmpleado_TipoArchivo_TipoArchivoId",
                        column: x => x.TipoArchivoId,
                        principalTable: "TipoArchivo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "TipoArchivo",
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

            migrationBuilder.CreateIndex(
                name: "IX_ArchivosEmpleado_EmpleadoId",
                table: "ArchivosEmpleado",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_ArchivosEmpleado_TipoArchivoId",
                table: "ArchivosEmpleado",
                column: "TipoArchivoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactosEmergencia_Empleados_EmpleadoId",
                table: "ContactosEmergencia",
                column: "EmpleadoId",
                principalTable: "Empleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactosEmergencia_Empleados_EmpleadoId",
                table: "ContactosEmergencia");

            migrationBuilder.DropTable(
                name: "ArchivosEmpleado");

            migrationBuilder.DropTable(
                name: "TipoArchivo");

            migrationBuilder.DropColumn(
                name: "ArchivoEmpleadoId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "IsPreregisterAuthorized",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PasswordResetNeeded",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Telefono",
                table: "Empleados",
                newName: "TelefonoPersonal");

            migrationBuilder.RenameColumn(
                name: "SegundoNombre",
                table: "Empleados",
                newName: "RFC");

            migrationBuilder.RenameColumn(
                name: "PrimerNombre",
                table: "Empleados",
                newName: "NSS");

            migrationBuilder.AddColumn<string>(
                name: "CURP",
                table: "Empleados",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                    EmpleadoId = table.Column<int>(type: "int", nullable: true),
                    FileTypeId = table.Column<int>(type: "int", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFiles_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserFiles_FileType_FileTypeId",
                        column: x => x.FileTypeId,
                        principalTable: "FileType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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

            migrationBuilder.CreateIndex(
                name: "IX_UserFiles_EmpleadoId",
                table: "UserFiles",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFiles_FileTypeId",
                table: "UserFiles",
                column: "FileTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactosEmergencia_Empleados_EmpleadoId",
                table: "ContactosEmergencia",
                column: "EmpleadoId",
                principalTable: "Empleados",
                principalColumn: "Id");
        }
    }
}
