using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class nombreEmpleado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimerNombre",
                table: "Empleados");

            migrationBuilder.RenameColumn(
                name: "SegundoNombre",
                table: "Empleados",
                newName: "Nombre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Empleados",
                newName: "SegundoNombre");

            migrationBuilder.AddColumn<string>(
                name: "PrimerNombre",
                table: "Empleados",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
