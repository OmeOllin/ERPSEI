using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class accesosModulos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccesoModulo_AspNetRoles_RolId",
                table: "AccesoModulo");

            migrationBuilder.DropForeignKey(
                name: "FK_AccesoModulo_Modulos_ModuloId",
                table: "AccesoModulo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccesoModulo",
                table: "AccesoModulo");

            migrationBuilder.RenameTable(
                name: "AccesoModulo",
                newName: "AccesosModulos");

            migrationBuilder.RenameIndex(
                name: "IX_AccesoModulo_RolId",
                table: "AccesosModulos",
                newName: "IX_AccesosModulos_RolId");

            migrationBuilder.RenameIndex(
                name: "IX_AccesoModulo_ModuloId",
                table: "AccesosModulos",
                newName: "IX_AccesosModulos_ModuloId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccesosModulos",
                table: "AccesosModulos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccesosModulos_AspNetRoles_RolId",
                table: "AccesosModulos",
                column: "RolId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccesosModulos_Modulos_ModuloId",
                table: "AccesosModulos",
                column: "ModuloId",
                principalTable: "Modulos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccesosModulos_AspNetRoles_RolId",
                table: "AccesosModulos");

            migrationBuilder.DropForeignKey(
                name: "FK_AccesosModulos_Modulos_ModuloId",
                table: "AccesosModulos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccesosModulos",
                table: "AccesosModulos");

            migrationBuilder.RenameTable(
                name: "AccesosModulos",
                newName: "AccesoModulo");

            migrationBuilder.RenameIndex(
                name: "IX_AccesosModulos_RolId",
                table: "AccesoModulo",
                newName: "IX_AccesoModulo_RolId");

            migrationBuilder.RenameIndex(
                name: "IX_AccesosModulos_ModuloId",
                table: "AccesoModulo",
                newName: "IX_AccesoModulo_ModuloId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccesoModulo",
                table: "AccesoModulo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccesoModulo_AspNetRoles_RolId",
                table: "AccesoModulo",
                column: "RolId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccesoModulo_Modulos_ModuloId",
                table: "AccesoModulo",
                column: "ModuloId",
                principalTable: "Modulos",
                principalColumn: "Id");
        }
    }
}
