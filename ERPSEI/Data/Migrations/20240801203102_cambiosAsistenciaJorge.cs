using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class cambiosAsistenciaJorge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Horario",
                table: "Asistencias");

            migrationBuilder.DropColumn(
                name: "NombreEmpleado",
                table: "Asistencias");

            migrationBuilder.AlterColumn<int>(
                name: "HorarioId",
                table: "Asistencias",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EmpleadoId",
                table: "Asistencias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_EmpleadoId",
                table: "Asistencias",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_HorarioId",
                table: "Asistencias",
                column: "HorarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asistencias_Empleados_EmpleadoId",
                table: "Asistencias",
                column: "EmpleadoId",
                principalTable: "Empleados",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Asistencias_Horarios_HorarioId",
                table: "Asistencias",
                column: "HorarioId",
                principalTable: "Horarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asistencias_Empleados_EmpleadoId",
                table: "Asistencias");

            migrationBuilder.DropForeignKey(
                name: "FK_Asistencias_Horarios_HorarioId",
                table: "Asistencias");

            migrationBuilder.DropIndex(
                name: "IX_Asistencias_EmpleadoId",
                table: "Asistencias");

            migrationBuilder.DropIndex(
                name: "IX_Asistencias_HorarioId",
                table: "Asistencias");

            migrationBuilder.DropColumn(
                name: "EmpleadoId",
                table: "Asistencias");

            migrationBuilder.AlterColumn<int>(
                name: "HorarioId",
                table: "Asistencias",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Horario",
                table: "Asistencias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreEmpleado",
                table: "Asistencias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
