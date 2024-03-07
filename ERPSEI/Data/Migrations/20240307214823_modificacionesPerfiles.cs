using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class modificacionesPerfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductosServicios_Perfiles_PerfilId",
                table: "ProductosServicios");

            migrationBuilder.DropIndex(
                name: "IX_ProductosServicios_PerfilId",
                table: "ProductosServicios");

            migrationBuilder.DropColumn(
                name: "PerfilId",
                table: "ProductosServicios");

            migrationBuilder.CreateTable(
                name: "ProductosServiciosPerfil",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PerfilId = table.Column<int>(type: "int", nullable: true),
                    ProductoServicioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductosServiciosPerfil", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductosServiciosPerfil_Perfiles_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductosServiciosPerfil_ProductosServicios_ProductoServicioId",
                        column: x => x.ProductoServicioId,
                        principalTable: "ProductosServicios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductosServiciosPerfil_PerfilId",
                table: "ProductosServiciosPerfil",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosServiciosPerfil_ProductoServicioId",
                table: "ProductosServiciosPerfil",
                column: "ProductoServicioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductosServiciosPerfil");

            migrationBuilder.AddColumn<int>(
                name: "PerfilId",
                table: "ProductosServicios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductosServicios_PerfilId",
                table: "ProductosServicios",
                column: "PerfilId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosServicios_Perfiles_PerfilId",
                table: "ProductosServicios",
                column: "PerfilId",
                principalTable: "Perfiles",
                principalColumn: "Id");
        }
    }
}
