using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class cambiosPrefacturas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prefacturas_AspNetUsers_UsuarioAutorizadorId",
                table: "Prefacturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Prefacturas_AspNetUsers_UsuarioFinalizadorId",
                table: "Prefacturas");

            migrationBuilder.DropIndex(
                name: "IX_Prefacturas_UsuarioAutorizadorId",
                table: "Prefacturas");

            migrationBuilder.DropColumn(
                name: "UsuarioAutorizadorId",
                table: "Prefacturas");

            migrationBuilder.RenameColumn(
                name: "UsuarioFinalizadorId",
                table: "Prefacturas",
                newName: "UsuarioTimbradorId");

            migrationBuilder.RenameIndex(
                name: "IX_Prefacturas_UsuarioFinalizadorId",
                table: "Prefacturas",
                newName: "IX_Prefacturas_UsuarioTimbradorId");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHoraCreacion",
                table: "Prefacturas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHoraTimbrado",
                table: "Prefacturas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequiereAutorizacion",
                table: "Prefacturas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AutorizacionesPrefactura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PrefacturaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaHoraAutorizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutorizacionesPrefactura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutorizacionesPrefactura_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AutorizacionesPrefactura_Prefacturas_PrefacturaId",
                        column: x => x.PrefacturaId,
                        principalTable: "Prefacturas",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "EstatusPrefactura",
                keyColumn: "Id",
                keyValue: 3,
                column: "Descripcion",
                value: "Timbrada");

            migrationBuilder.CreateIndex(
                name: "IX_AutorizacionesPrefactura_PrefacturaId",
                table: "AutorizacionesPrefactura",
                column: "PrefacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_AutorizacionesPrefactura_UsuarioId",
                table: "AutorizacionesPrefactura",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prefacturas_AspNetUsers_UsuarioTimbradorId",
                table: "Prefacturas",
                column: "UsuarioTimbradorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prefacturas_AspNetUsers_UsuarioTimbradorId",
                table: "Prefacturas");

            migrationBuilder.DropTable(
                name: "AutorizacionesPrefactura");

            migrationBuilder.DropColumn(
                name: "FechaHoraCreacion",
                table: "Prefacturas");

            migrationBuilder.DropColumn(
                name: "FechaHoraTimbrado",
                table: "Prefacturas");

            migrationBuilder.DropColumn(
                name: "RequiereAutorizacion",
                table: "Prefacturas");

            migrationBuilder.RenameColumn(
                name: "UsuarioTimbradorId",
                table: "Prefacturas",
                newName: "UsuarioFinalizadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Prefacturas_UsuarioTimbradorId",
                table: "Prefacturas",
                newName: "IX_Prefacturas_UsuarioFinalizadorId");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioAutorizadorId",
                table: "Prefacturas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "EstatusPrefactura",
                keyColumn: "Id",
                keyValue: 3,
                column: "Descripcion",
                value: "Finalizada");

            migrationBuilder.CreateIndex(
                name: "IX_Prefacturas_UsuarioAutorizadorId",
                table: "Prefacturas",
                column: "UsuarioAutorizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prefacturas_AspNetUsers_UsuarioAutorizadorId",
                table: "Prefacturas",
                column: "UsuarioAutorizadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Prefacturas_AspNetUsers_UsuarioFinalizadorId",
                table: "Prefacturas",
                column: "UsuarioFinalizadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
