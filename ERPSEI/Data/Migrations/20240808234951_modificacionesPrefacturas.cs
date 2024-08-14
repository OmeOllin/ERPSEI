using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class modificacionesPrefacturas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutorizacionesPrefactura_AspNetUsers_UsuarioId",
                table: "AutorizacionesPrefactura");

            migrationBuilder.DropForeignKey(
                name: "FK_AutorizacionesPrefactura_Prefacturas_PrefacturaId",
                table: "AutorizacionesPrefactura");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AutorizacionesPrefactura",
                table: "AutorizacionesPrefactura");

            migrationBuilder.RenameTable(
                name: "AutorizacionesPrefactura",
                newName: "AutorizacionesPrefacturas");

            migrationBuilder.RenameIndex(
                name: "IX_AutorizacionesPrefactura_UsuarioId",
                table: "AutorizacionesPrefacturas",
                newName: "IX_AutorizacionesPrefacturas_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_AutorizacionesPrefactura_PrefacturaId",
                table: "AutorizacionesPrefacturas",
                newName: "IX_AutorizacionesPrefacturas_PrefacturaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AutorizacionesPrefacturas",
                table: "AutorizacionesPrefacturas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AutorizacionesPrefacturas_AspNetUsers_UsuarioId",
                table: "AutorizacionesPrefacturas",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AutorizacionesPrefacturas_Prefacturas_PrefacturaId",
                table: "AutorizacionesPrefacturas",
                column: "PrefacturaId",
                principalTable: "Prefacturas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutorizacionesPrefacturas_AspNetUsers_UsuarioId",
                table: "AutorizacionesPrefacturas");

            migrationBuilder.DropForeignKey(
                name: "FK_AutorizacionesPrefacturas_Prefacturas_PrefacturaId",
                table: "AutorizacionesPrefacturas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AutorizacionesPrefacturas",
                table: "AutorizacionesPrefacturas");

            migrationBuilder.RenameTable(
                name: "AutorizacionesPrefacturas",
                newName: "AutorizacionesPrefactura");

            migrationBuilder.RenameIndex(
                name: "IX_AutorizacionesPrefacturas_UsuarioId",
                table: "AutorizacionesPrefactura",
                newName: "IX_AutorizacionesPrefactura_UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_AutorizacionesPrefacturas_PrefacturaId",
                table: "AutorizacionesPrefactura",
                newName: "IX_AutorizacionesPrefactura_PrefacturaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AutorizacionesPrefactura",
                table: "AutorizacionesPrefactura",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AutorizacionesPrefactura_AspNetUsers_UsuarioId",
                table: "AutorizacionesPrefactura",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AutorizacionesPrefactura_Prefacturas_PrefacturaId",
                table: "AutorizacionesPrefactura",
                column: "PrefacturaId",
                principalTable: "Prefacturas",
                principalColumn: "Id");
        }
    }
}
