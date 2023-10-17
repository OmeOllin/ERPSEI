using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Empleados_EmpleadoId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactosEmergencia_Empleados_EmpleadoId",
                table: "ContactosEmergencia");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Areas_AreaId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_EstadosCiviles_EstadoCivilId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Generos_GeneroId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Puestos_PuestoId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFiles_Empleados_EmpleadoId",
                table: "UserFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFiles_FileType_FileTypeId",
                table: "UserFiles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmpleadoId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "FileTypeId",
                table: "UserFiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EmpleadoId",
                table: "UserFiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PuestoId",
                table: "Empleados",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "GeneroId",
                table: "Empleados",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EstadoCivilId",
                table: "Empleados",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AreaId",
                table: "Empleados",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Empleados",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmpleadoId",
                table: "ContactosEmergencia",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EmpleadoId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmpleadoId",
                table: "AspNetUsers",
                column: "EmpleadoId",
                unique: true,
                filter: "[EmpleadoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Empleados_EmpleadoId",
                table: "AspNetUsers",
                column: "EmpleadoId",
                principalTable: "Empleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactosEmergencia_Empleados_EmpleadoId",
                table: "ContactosEmergencia",
                column: "EmpleadoId",
                principalTable: "Empleados",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Areas_AreaId",
                table: "Empleados",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_EstadosCiviles_EstadoCivilId",
                table: "Empleados",
                column: "EstadoCivilId",
                principalTable: "EstadosCiviles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Generos_GeneroId",
                table: "Empleados",
                column: "GeneroId",
                principalTable: "Generos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Puestos_PuestoId",
                table: "Empleados",
                column: "PuestoId",
                principalTable: "Puestos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFiles_Empleados_EmpleadoId",
                table: "UserFiles",
                column: "EmpleadoId",
                principalTable: "Empleados",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFiles_FileType_FileTypeId",
                table: "UserFiles",
                column: "FileTypeId",
                principalTable: "FileType",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Empleados_EmpleadoId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactosEmergencia_Empleados_EmpleadoId",
                table: "ContactosEmergencia");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Areas_AreaId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_EstadosCiviles_EstadoCivilId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Generos_GeneroId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Puestos_PuestoId",
                table: "Empleados");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFiles_Empleados_EmpleadoId",
                table: "UserFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFiles_FileType_FileTypeId",
                table: "UserFiles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmpleadoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Empleados");

            migrationBuilder.AlterColumn<int>(
                name: "FileTypeId",
                table: "UserFiles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmpleadoId",
                table: "UserFiles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PuestoId",
                table: "Empleados",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GeneroId",
                table: "Empleados",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EstadoCivilId",
                table: "Empleados",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AreaId",
                table: "Empleados",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmpleadoId",
                table: "ContactosEmergencia",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmpleadoId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmpleadoId",
                table: "AspNetUsers",
                column: "EmpleadoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Empleados_EmpleadoId",
                table: "AspNetUsers",
                column: "EmpleadoId",
                principalTable: "Empleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactosEmergencia_Empleados_EmpleadoId",
                table: "ContactosEmergencia",
                column: "EmpleadoId",
                principalTable: "Empleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Areas_AreaId",
                table: "Empleados",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_EstadosCiviles_EstadoCivilId",
                table: "Empleados",
                column: "EstadoCivilId",
                principalTable: "EstadosCiviles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Generos_GeneroId",
                table: "Empleados",
                column: "GeneroId",
                principalTable: "Generos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Puestos_PuestoId",
                table: "Empleados",
                column: "PuestoId",
                principalTable: "Puestos",
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
    }
}
