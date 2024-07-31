using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPSEI.Data.Migrations
{
    /// <inheritdoc />
    public partial class cambiosEntidadAsistencias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaHora",
                table: "Asistencias");

            migrationBuilder.RenameColumn(
                name: "SerialDispositivo",
                table: "Asistencias",
                newName: "ResultadoS");

            migrationBuilder.RenameColumn(
                name: "NombreDispositivo",
                table: "Asistencias",
                newName: "ResultadoE");

            migrationBuilder.RenameColumn(
                name: "NoTarjeta",
                table: "Asistencias",
                newName: "Horario");

            migrationBuilder.RenameColumn(
                name: "Hora",
                table: "Asistencias",
                newName: "Salida");

            migrationBuilder.RenameColumn(
                name: "Direccion",
                table: "Asistencias",
                newName: "Dia");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Entrada",
                table: "Asistencias",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Entrada",
                table: "Asistencias");

            migrationBuilder.RenameColumn(
                name: "Salida",
                table: "Asistencias",
                newName: "Hora");

            migrationBuilder.RenameColumn(
                name: "ResultadoS",
                table: "Asistencias",
                newName: "SerialDispositivo");

            migrationBuilder.RenameColumn(
                name: "ResultadoE",
                table: "Asistencias",
                newName: "NombreDispositivo");

            migrationBuilder.RenameColumn(
                name: "Horario",
                table: "Asistencias",
                newName: "NoTarjeta");

            migrationBuilder.RenameColumn(
                name: "Dia",
                table: "Asistencias",
                newName: "Direccion");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHora",
                table: "Asistencias",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
