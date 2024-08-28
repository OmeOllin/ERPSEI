using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class modificacionesHorariosAsistencias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asistencias_Horarios_HorarioId",
                table: "Asistencias");

            migrationBuilder.DropIndex(
                name: "IX_Asistencias_HorarioId",
                table: "Asistencias");

            migrationBuilder.DropColumn(
                name: "Entrada",
                table: "Horarios");

            migrationBuilder.DropColumn(
                name: "Salida",
                table: "Horarios");

            migrationBuilder.DropColumn(
                name: "ToleranciaEntrada",
                table: "Horarios");

            migrationBuilder.DropColumn(
                name: "ToleranciaFalta",
                table: "Horarios");

            migrationBuilder.DropColumn(
                name: "ToleranciaSalida",
                table: "Horarios");

            migrationBuilder.DropColumn(
                name: "HorarioId",
                table: "Asistencias");

            migrationBuilder.RenameColumn(
                name: "NombreHorario",
                table: "Horarios",
                newName: "Descripcion");

            migrationBuilder.AddColumn<int>(
                name: "HorarioId",
                table: "Empleados",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HorarioDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    HorarioId = table.Column<int>(type: "int", nullable: false),
                    NumeroDiaSemana = table.Column<int>(type: "int", nullable: false),
                    Entrada = table.Column<TimeSpan>(type: "time", nullable: false),
                    ToleranciaEntrada = table.Column<TimeSpan>(type: "time", nullable: false),
                    ToleranciaFalta = table.Column<TimeSpan>(type: "time", nullable: false),
                    Salida = table.Column<TimeSpan>(type: "time", nullable: false),
                    ToleranciaSalida = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorarioDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorarioDetalle_Horarios_HorarioId",
                        column: x => x.HorarioId,
                        principalTable: "Horarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_HorarioId",
                table: "Empleados",
                column: "HorarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HorarioDetalle_HorarioId",
                table: "HorarioDetalle",
                column: "HorarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_Horarios_HorarioId",
                table: "Empleados",
                column: "HorarioId",
                principalTable: "Horarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_Horarios_HorarioId",
                table: "Empleados");

            migrationBuilder.DropTable(
                name: "HorarioDetalle");

            migrationBuilder.DropIndex(
                name: "IX_Empleados_HorarioId",
                table: "Empleados");

            migrationBuilder.DropColumn(
                name: "HorarioId",
                table: "Empleados");

            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Horarios",
                newName: "NombreHorario");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Entrada",
                table: "Horarios",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Salida",
                table: "Horarios",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ToleranciaEntrada",
                table: "Horarios",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ToleranciaFalta",
                table: "Horarios",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ToleranciaSalida",
                table: "Horarios",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "HorarioId",
                table: "Asistencias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Asistencias_HorarioId",
                table: "Asistencias",
                column: "HorarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asistencias_Horarios_HorarioId",
                table: "Asistencias",
                column: "HorarioId",
                principalTable: "Horarios",
                principalColumn: "Id");
        }
    }
}
